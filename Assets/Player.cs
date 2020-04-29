using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour{

    //Student Variables
    public string studentName;
    public int studentNumber;
    public string levelName;

    public float moveSpeed;

    public float angle;
    public float gravity;
    public float velocity;

    #region Controlled Variables
    Rigidbody rb;

    private float distance;
    private float height;

    private Vector3 origin;
    private Vector3 newPosition;

    //Jumping Variables
    private float distToGround = 1;
    private bool jumping = false;

    //Model 
    public GameObject model;
    public Animator anim;
    private Quaternion newRotation;
    #endregion

    // Start is called before the first frame update
    void Start() {

        rb = GetComponent<Rigidbody>();

        distToGround = transform.GetComponent<Collider>().bounds.extents.y;

        Physics.gravity = new Vector3(0, -gravity, 0);

        origin = transform.position;
        newPosition = origin;
    }

    #region Update
    // Update is called once per frame
    void Update() {

        //Movement
        if (!jumping && IsGrounded())
            Movement();

        //Jumping
        if (Input.GetKey("space") && !jumping && IsGrounded()) {
            //Min velocity to jump
            if (rb.velocity.x > 2 || rb.velocity.x < -2) {
                jumping = true;
                anim.SetBool("Jumping", true);
            } 
        }

        //Actual Jumping method call
        if (jumping) {
            Jumping();
        }

        //Falling
        if (!IsGrounded()) {
            anim.SetBool("Falling", true);
        } else {
            anim.SetBool("Falling", false);
        }
    }
    #endregion

    #region Movement
    void Movement() {

        if (Input.GetKey("a") ) {
            rb.AddRelativeForce(transform.right * moveSpeed * Time.deltaTime);
            velocity = rb.velocity.x;
            origin = transform.position;
            anim.SetBool("Running", true);
            newRotation.eulerAngles = new Vector3(newRotation.eulerAngles.x, -1 * 90, newRotation.eulerAngles.z);
            model.transform.rotation = newRotation;
        }
        else if (Input.GetKey("d")) {
            rb.AddRelativeForce(transform.right * -moveSpeed * Time.deltaTime);
            velocity = rb.velocity.x;
            origin = transform.position;
            anim.SetBool("Running", true);
            newRotation.eulerAngles = new Vector3(newRotation.eulerAngles.x, 1 * 90, newRotation.eulerAngles.z);
            model.transform.rotation = newRotation;
        }
        else {
            anim.SetBool("Running", false);
        }
    }
    #endregion

    //Ballistics Equation controlling jump displacement
    void Jumping() {
        
        //Move forward
        transform.position += transform.forward * (velocity) * Time.deltaTime;

        //Calculate correct X axis displacement - account for pythagaros theorem
        Vector3 tempPos = transform.position;
        tempPos.y = origin.y;
        distance = Vector3.Distance(origin, tempPos);

        //Displacement formula
        height = (distance * Mathf.Tan(angle)) - (gravity * Mathf.Pow(distance, 2)) /
                 (2 * Mathf.Pow(velocity, 2) * Mathf.Pow(Mathf.Cos(angle), 2));

        //Recorrect height
        newPosition = new Vector3(transform.position.x, origin.y + height, transform.position.z);
        if (!float.IsNaN(newPosition.y) && !float.IsInfinity(newPosition.y))
            transform.position = newPosition;
    }

    #region Ground Checks
    //Reset jumping bool upon colliding with Ground
    private void OnCollisionEnter(Collision collision) {

        if (collision.gameObject.tag == "Ground") {
            jumping = false;
            anim.SetBool("Jumping", false);
        }
    }

    // Grounded Check - 3 short raycasts - straight down, slightly left, slight right
    private bool IsGrounded() {

        Vector3 sLeft = transform.position;
        sLeft.x = sLeft.x - 0.25f;
        Vector3 sRight = transform.position;
        sRight.x = sRight.x + 0.25f;

        if (Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.2f))
            return true;
        else if (Physics.Raycast(sLeft, -Vector3.up, distToGround + 0.2f))
            return true;
        else if (Physics.Raycast(sRight, -Vector3.up, distToGround + 0.2f))
            return true;
        else
            return false;
    }
    #endregion
}

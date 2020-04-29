using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {

    public GameObject player;

    private Vector3 focusPoint;

    private Vector3 cameraPosition;

    //Rotation vars
    private float rotationSpeed = 0.05f;
    private float adjRotSpeed;
    private Quaternion targetRotation;
	
	// Update is called once per frame
	void Update () {

        focusPoint = player.transform.position;

        //Lerp focus towards focusPoint
        targetRotation = Quaternion.LookRotation(focusPoint - transform.position);
        adjRotSpeed = Mathf.Min(rotationSpeed * Time.deltaTime, 1);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, adjRotSpeed);

        //Camera Position Setup
        cameraPosition = focusPoint;
        cameraPosition.z = cameraPosition.z - 10.0f;
        cameraPosition.y = cameraPosition.y + 1.00f;

        //Move Y position to mid point
        transform.position = Vector3.MoveTowards(transform.position, cameraPosition, 15.0f * Time.deltaTime);
    }
}

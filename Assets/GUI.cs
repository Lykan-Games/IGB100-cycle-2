using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI : MonoBehaviour{

    Player player;

    public Text studentName;
    public Text studentNumber;
    public Text levelName;
    public Text gravity;
    public Text jumpAngle;
    public Text velocity;

    // Start is called before the first frame update
    void Start() {

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        studentName.text = "Student: " + player.studentName;
        studentNumber.text = "Number: " + player.studentNumber.ToString();
        levelName.text = "Level: " + player.levelName;

        gravity.text = "Gravity: " + player.gravity.ToString();
        jumpAngle.text = "Jump Angle: " + (player.angle * 180 / Mathf.PI).ToString();
    }

    // Update is called once per frame
    void Update(){
        velocity.text = "Velocity: " + player.transform.GetComponent<Rigidbody>().velocity;
    }
}

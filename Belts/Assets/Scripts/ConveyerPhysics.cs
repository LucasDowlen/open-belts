using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyerPhysics : MonoBehaviour
{

    public Rigidbody2D rb;
    private GameObject effector;
    public float speed = .9f;
    // public float turnTimer = .7f;
    public float tempTime;
    private bool turnRight;
    private bool turnDown;



    // void Start() {
        // tempTime = turnTimer;
        // turnTimer /= speed;
    // }

    // void Update() {

        // if(turnRight && turnDown) return;

        // else if(turnRight == true) {
        //     if(tempTime > 0) {
        //         tempTime -= Time.deltaTime;
        //         rb.velocity = new Vector3(0, speed);
        //     }
        //     else {
        //         // Debug.Log("!>");
        //         rb.velocity = new Vector3(speed, 0);
        //         turnRight = false;
        //         // tempTime = turnTimer;
        //         //set back after exiting collider;
        //     }
        // }

        // else if(turnDown == true) {
        //     if(tempTime > 0) {
        //         tempTime -= Time.deltaTime;
        //         rb.velocity = new Vector3(speed, 0);
        //     }
        //     else {
        //         rb.velocity = new Vector3(0, -speed);
        //         turnDown = false;
        //     }
        // }
    // }

    void FixedUpdate() {
        RaycastHit2D positionRay = Physics2D.Linecast(transform.position, transform.position);


        //shorten if statement -- lengthened for testing
        if(positionRay.collider != null && positionRay.collider.gameObject != null && positionRay.collider.gameObject.GetComponent<BeltCollision>() != null) {
            
            // Debug.Log("Collides");

            // Debug.Log("GameObject: " + positionRay.collider.gameObject);

            effector = positionRay.collider.gameObject;


            if (effector.GetComponent<BeltCollision>().direction == 0) rb.velocity = new Vector3(0, speed); //up

            else if(effector.GetComponent<BeltCollision>().direction == 1) rb.velocity = new Vector3(speed, 0); //right

            else if(effector.GetComponent<BeltCollision>().direction == 2) rb.velocity = new Vector3(0, -speed); //down

            else if(effector.GetComponent<BeltCollision>().direction == 3) rb.velocity = new Vector3(-speed, 0); //left
        }

        else {
            // Debug.Log("No Collision");
            rb.velocity = new Vector3(0, 0);
        }
    }

    // private void OnTriggerStay2D(Collider2D collider) {

    //     effector = collider.gameObject;

    //     // if(effector.GetComponent<BeltCollision>() == null) return;

    //     if (effector.GetComponent<BeltCollision>().direction == 0) rb.velocity = new Vector3(0, speed); //up

    //     else if(effector.GetComponent<BeltCollision>().direction == 1) rb.velocity = new Vector3(speed, 0); //right

    //     else if(effector.GetComponent<BeltCollision>().direction == 2) rb.velocity = new Vector3(0, -speed); //down

    //     else if(effector.GetComponent<BeltCollision>().direction == 3) rb.velocity = new Vector3(-speed, 0); //left

    //     // else if(effector.GetComponent<BeltCollision>().direction == 1.1f) turnRight = true;

    //     // else if(effector.GetComponent<BeltCollision>().direction == 2.1f) turnDown = true;
    // }

    private void OnTriggerExit2D() { 

        // tempTime = turnTimer; //kinda works (fix later)
        // turnDown = false;
        // turnRight = false;

        // if(tempTime <= 0) {
        //     tempTime = turnTimer; 
        //     turnRight = false;
        // }

        // effector = collider.gameObject;

        // if(effector.GetComponent<BeltCollision>() != null && effector.GetComponent<BeltCollision>().direction == 1) return;

        rb.velocity = new Vector3(0, 0);
    }

    // private void OnTriggerEnter2D() {
    //     tempTime = turnTimer;
    // }
}

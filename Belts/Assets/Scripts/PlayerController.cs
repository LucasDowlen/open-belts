using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed;
    public Rigidbody2D rb;
    public Animator anim;
//not showing in inspector??

    private bool verticalPriority;

    private float hMove;
    private float vMove;
    private bool orientedHorizontal;

    void Start()
    {
        
    }

    void Update()
    {

        if(Input.GetKeyDown("w") || Input.GetKeyDown("s")) {
            verticalPriority = true;
        }
        else if((Input.GetKeyUp("w") || Input.GetKeyUp("s")) && (Input.GetKey("a") || Input.GetKey("d"))) {
            verticalPriority = false;
            // Debug.Log("Pass");
        }
        else if(Input.GetKeyDown("a") || Input.GetKeyDown("d")) {
            verticalPriority = false;
        }


        if(Input.GetAxisRaw("Horizontal") != 0 && !verticalPriority) {
            hMove = Input.GetAxisRaw("Horizontal");
            vMove = 0;

            anim.SetBool("HorizontalRun", true);
            anim.SetBool("HorizontalStill", false);
            orientedHorizontal = true;
            

            if(Input.GetAxisRaw("Horizontal") == -1) {
                transform.eulerAngles = new Vector3(0, 180, 0); //swaped angles
            }
            else if(Input.GetAxisRaw("Horizontal") == 1) {
                transform.eulerAngles = new Vector3(0, 0, 0); //swaped angles
            }
        }
        
        else if(Input.GetAxisRaw("Vertical") != 0) {
            vMove = Input.GetAxisRaw("Vertical");
            hMove = 0;
            anim.SetBool("HorizontalRun", false);
            anim.SetBool("HorizontalStill", false);
            orientedHorizontal = false;

        }
        else {
            hMove = 0;
            vMove = 0;

            if(orientedHorizontal) {
                anim.SetBool("HorizontalStill", true);
            }
            else {
                anim.SetBool("HorizontalStill", false);
            }
        }

        Vector3 velocity = new Vector3(hMove, vMove);
        rb.velocity = velocity;
    }
}

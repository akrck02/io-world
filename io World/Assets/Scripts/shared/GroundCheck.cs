using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
   
    public Transform groundCheck;
    public Rigidbody2D rigibody;

    public LayerMask groundLayer;
   

    /**
    * Gets if object is grounded
    */
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }


       /**
    * Update is called once per frame
    */
    void Update()
    {
       if(IsGrounded()){
        rigibody.velocity = new Vector2(0,0);
       }
    }



}

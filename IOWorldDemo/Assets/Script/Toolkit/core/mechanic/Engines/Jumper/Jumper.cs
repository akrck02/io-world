using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Jumper : MonoBehaviour
{

    private readonly bool DEFAULT_ENABLED = true;
    public bool enabled;
    public float jumpForce = 10f;
    public float jumpTime = 0.5f;
    public float rotation = 0;


    // Start is called before the first frame update
    void Start()
    {
        if(this.enabled == null) {
            this.enabled = DEFAULT_ENABLED;
        }

        if(gameObject.GetComponent<BoxCollider2D>() == null) {
            gameObject.AddComponent<BoxCollider2D>();
        }
      
    }

    // Update is called once per frameb
    void Update()
    {
      UpdateRotation();  
    }

    void UpdateRotation(){
        this.rotation = Geometrics.GetLocalRotation(gameObject);
    }

    void OnCollisionEnter2D(Collision2D other) 
    {
       // if (other.gameObject.name == "Player" && this.enabled)
        if(true)
        {
            Jump(other);
        }
    }

    void Jump(Collision2D other)
    {
       
        Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();

        // Check axis and set velocity
        Vector2 axis = Geometrics.CalculateAxis(rotation);

        Debug.Log("ROTATION: " + rotation);
        Debug.Log("AXIS: " + axis);

        // Set item velocity
        Vector2 velocity = axis * jumpForce;
        rb.velocity = velocity;

        Debug.Log(velocity);

    }


}

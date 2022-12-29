using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attraction : MonoBehaviour
{

    public GameObject attractor;
    public float speed = 1f;
    public float acceleration = 2f;

    public Rigidbody2D rb;
    public float lastSpeed = 1f;

    public float maxSpeed = 10f;

    public float margin = .7f;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lastSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        
       //if not near the attractor
        if (Vector3.Distance(transform.position, attractor.transform.position) > margin)
        {
           
            if (lastSpeed > maxSpeed)
            {
                lastSpeed = maxSpeed;
            } else {
                lastSpeed += lastSpeed * acceleration;
            }

            transform.position = Vector3.MoveTowards(transform.position, attractor.transform.position, lastSpeed * Time.deltaTime);

        }
        else
        {
            // stop completely
            lastSpeed = 0f;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.Sleep();

        }



    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectStats : MonoBehaviour
{

    public Vector2 velocity;
    public Rigidbody2D rigidbody;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(rigidbody != null) {
            velocity = rigidbody.velocity;
        }
    }
}

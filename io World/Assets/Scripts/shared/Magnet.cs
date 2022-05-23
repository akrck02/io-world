using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{

    //get circle collider 2d    
    [SerializeField] private CircleCollider2D circleCollider;
    [SerializeField] private GameObject magnet;
    [SerializeField] private float magnetRange;
    [SerializeField] private float magnetStrength;

    private GameObject attractor;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(attractor != null)
        {
           // Vector3 direction = (attractor.transform.position - magnet.transform.position).normalized;

           
        }

    }

    //OnCollisionEnter2D if the material is a magnet
    void OnTriggerEnter2D(Collider2D collision){

        if (collision.gameObject.tag == "Magnet-focus")
        {
            Debug.Log("Moving magnet");
            attractor = collision.gameObject;
        }

    }

     
     private void OnTriggerExit2D (Collider2D other) {
         if (other.gameObject.tag == "Magnet-focus") {
             attractor = null;
         }
     }
}

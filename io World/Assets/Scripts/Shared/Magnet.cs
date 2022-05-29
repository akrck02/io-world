using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    private GameObject attractor;

    //OnCollisionEnter2D if the material is a magnet
    void OnTriggerEnter2D(Collider2D collision){

        if (collision.gameObject.tag == "Magnet-focus")
        {
             //set rotation to 0
            transform.rotation = Quaternion.identity; 

            attractor = collision.gameObject;
        }

    }
     
    void OnTriggerExit2D (Collider2D other) {
         
         if (other.gameObject.tag == "Magnet-focus") {
             attractor = null;
         }
     }
}

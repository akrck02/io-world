using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrabController : MonoBehaviour
{
    [SerializeField]
    private Transform boxHolder;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private CircleCollider2D grabCollider;

    [SerializeField]
    private GameObject currentObject;

    void OnTriggerEnter2D(Collider2D collider) {

        if (collider.tag == "Grab") {
            currentObject = collider.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D collider){

        if(collider.gameObject == currentObject) {
            currentObject = null;
        }

    }

    
    public void Grab(){

        /*
        bool isMagnet = collider.gameObject.layer == 7;

            Debug.Log(collider.GetContacts(new ContactPoint2D[]{}));

            if (Input.GetKey(KeyCode.E)) {
                collider.gameObject.transform.parent = boxHolder;
                collider.gameObject.transform.position = boxHolder.position;
                collider.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;

                if (isMagnet) {
                    player.layer = 7;
                }
            }
            else {
                collider.gameObject.transform.parent = null;
                collider.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;

                // add force to vector to expulse object
                Vector2 force = new Vector2(0, 0);
                force.x = -1 * collider.gameObject.GetComponent<Rigidbody2D>().velocity.x;
                force.y = -1 * collider.gameObject.GetComponent<Rigidbody2D>().velocity.y;

                collider.gameObject.GetComponent<Rigidbody2D>().AddForce(force);
                player.layer = 0;
            }

        */

        Debug.Log("Grab Remote!");
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabController : MonoBehaviour
{

    [SerializeField] private Transform grabDetect;
    [SerializeField] private Transform boxHolder;
    [SerializeField] private GameObject player;
    [SerializeField] private float rayDist;

    // Update is called once per frame
    void Update()
    {
        
        RaycastHit2D grabCheck = Physics2D.Raycast(grabDetect.position, Vector2.right * transform.localScale, rayDist);

        if(grabCheck.collider != null && grabCheck.collider.tag == "Grab")
        {

            bool isMagnet = grabCheck.collider.gameObject.layer == 7;
            
            
            if(Input.GetKey(KeyCode.E)){
                grabCheck.collider.gameObject.transform.parent = boxHolder;
                grabCheck.collider.gameObject.transform.position = boxHolder.position;
                grabCheck.collider.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;    

                if(isMagnet){
                    player.layer = 7;  
                }


            }
            else {
                grabCheck.collider.gameObject.transform.parent = null;
                grabCheck.collider.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;

                // add force to vector to expulse object
                Vector2 force = new Vector2(0, 0);
                force.x = -1 * grabCheck.collider.gameObject.GetComponent<Rigidbody2D>().velocity.x;
                force.y = -1 * grabCheck.collider.gameObject.GetComponent<Rigidbody2D>().velocity.y;
                grabCheck.collider.gameObject.GetComponent<Rigidbody2D>().AddForce(force);
                

                player.layer = 0;  
            }

        }

    }
}

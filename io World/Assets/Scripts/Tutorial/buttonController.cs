using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonController : MonoBehaviour
{
    public GameObject button;
    public GameObject platform;
    private bool platformMoving = false; 
    private Vector3 target;

    private buttonMode mode;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(platformMoving)
            movePlatform();
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        print(coll.gameObject.name);
        if (coll.gameObject.name == "button")
        {
            buttonMode mode = button.GetComponent<buttonMode>();
            mode.activate();

            if(!mode.active) {
                this.platformMoving = true;
                target = new Vector3(platform.transform.position.x, -1.74f, platform.transform.position.z); 
            }
            else {
                this.platformMoving = true;
                target = new Vector3(platform.transform.position.x, 2.5f, platform.transform.position.z);
            } 
        }
    }


    void movePlatform() {
        platform.transform.position = Vector2.MoveTowards(
                platform.transform.position,
                target,
                10 * Time.deltaTime
        );
    }

}

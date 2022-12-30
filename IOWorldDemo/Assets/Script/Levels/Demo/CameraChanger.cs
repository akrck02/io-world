using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChanger : MonoBehaviour
{

    public Camera mainCamera;
    public Camera secondCamera;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player") {

            mainCamera.enabled = false;
            secondCamera.enabled = true;
        }
    }
}

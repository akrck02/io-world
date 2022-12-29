using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{

    private readonly float SPEED = 10.8f;
    private float intensity;
    private bool influence;
    private Rigidbody2D rigidbody;
    

    // Start is called before the first frame update
    void Start()
    {
        this.intensity = 1f;
        this.rigidbody = GetComponent<Rigidbody2D>();
        this.StartInfluence();
    }

    // Stop gravity effect
    void StopInfluence() {
        Debug.Log("Stoping GRAVITY influence");
        this.influence = false;
    }

    // Start gravity effect
    void StartInfluence() {

        Debug.Log("Start of GRAVITY influence requested");

        if(this.rigidbody == null){
            this.StopInfluence();
            return;
        }

        Debug.Log("Starting GRAVITY influence");
        Debug.Log(this.rigidbody);
        this.influence = true;
    }


    // Set intensity
    void SetIntensity(float intensity) {
        this.intensity = intensity;
    }


    // Update is called once per frame
    void Update()
    {

        if(!influence){
            return;
        }

        this.pushDown(this.rigidbody);        
       
    }


    // Push down the given rigibody
    private void pushDown(Rigidbody2D rigidbody){

        //Debug.Log("Pushing down requested");

        if(rigidbody == null){
           // Debug.Log("No rigidbody detected");
            return;
        }

        
        Debug.Log("Pushing down");
        rigidbody.AddForce(new Vector2(0,-this.intensity * this.SPEED));
       
      
    }
}

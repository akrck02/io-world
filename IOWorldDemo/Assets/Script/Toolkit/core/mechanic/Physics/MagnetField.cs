using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MagnetField : MonoBehaviour
{

    private static readonly string[] attractableTags = new string[] { "Magnet", "Metalic" };


    public bool powerOn;
    public GameObject attractor;

    public float speed;

    public float acceleration;

    public float maxSpeed;

    public float margin;


    // Start is called before the first frame update
    void Start()
    {

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        //if pw is off, don't do anything
        if(!this.powerOn)
        {
            return;
        }

        // If the object is not a attractable, don't do anything
        if(Array.IndexOf(attractableTags,collider.tag) == -1)
        {
            return;
        }

        // Add attraction script to the object
        collider.gameObject.AddComponent<Attraction>();
        Attraction attraction = collider.gameObject.GetComponent<Attraction>();
        attraction.attractor = this.attractor;
        attraction.speed = this.speed;
        attraction.acceleration = this.acceleration;
        attraction.maxSpeed = this.maxSpeed;
        attraction.margin = this.margin;
        
    }

    void OnTriggerExit2D(Collider2D collider)
    {

        // if pw is off, don't do anything
        if(!this.powerOn)
        {
            return;
        }

        // If the object is not a attractable, don't do anything
        if(Array.IndexOf(attractableTags,collider.tag) == -1)
        {
            return;
        }

    }
}

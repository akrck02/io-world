using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ButtonToggle : MonoBehaviour
{

    private readonly bool DEFAULT_STARTED = false;
    public bool started; 
    public IButtonToggleHandler handler;

    public string[] supportedTags = {"Magnet", "Player"};


    // Start is called before the first frame update
    void Start()
    {
        if(this.started == null) {
            this.started = DEFAULT_STARTED;
        }

        if(gameObject.GetComponent<SpriteRenderer>() == null) {
            gameObject.AddComponent<SpriteRenderer>();
        }

        if(gameObject.GetComponent<BoxCollider2D>() == null) {
            gameObject.AddComponent<BoxCollider2D>();
        }

        if(gameObject.GetComponent<Rigidbody2D>() == null) {
           gameObject.AddComponent<Rigidbody2D>();
           gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
           gameObject.GetComponent<Rigidbody2D>().simulated = true;
        }

    }


    void OnCollisionEnter2D(Collision2D coll){

        bool mayActivate = false;

        if(coll.gameObject.name == "Player"){
            mayActivate = true;
        }

        //if in supportedTags
        if(Array.Exists(supportedTags, element => element == coll.gameObject.tag)){
            mayActivate = true;
        }


        if(mayActivate && !this.started){
            this.on();
        } 
    }

    void OnCollisionExit2D(Collision2D coll) {

        bool mayDeactivate = false;

        if(coll.gameObject.name == "Player"){
            mayDeactivate = true;
        }

        if(mayDeactivate && this.started){
            this.off();
        }

    }


    void on() {
        
        this.handler.on();
        this.started = true;

    }

    void off() {
        
        this.handler.off();
        this.started = false;

    }

    // Update is called once per frame
    void Update() {
        
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClick : MonoBehaviour
{

    private readonly bool DEFAULT_STARTED = false;
    public bool started; 
    public IButtonClickHandler handler;


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

        if(mayActivate && !this.started){
            this.Activate();
        }
    }



    void Activate() {
        
        this.handler.Activate();
        this.started = true;

    }

    // Update is called once per frame
    void Update() {
        
    }

}

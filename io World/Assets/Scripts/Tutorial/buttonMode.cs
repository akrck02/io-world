using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonMode : MonoBehaviour
{

    public bool active = false;
    public Sprite on,off;
    public GameObject button;

    public void toggle()
    {
        if(active) deactivate();
        else activate();
        
    }

    public void activate()
    {
        this.active = true;
        SpriteRenderer sprite = button.GetComponent<SpriteRenderer>();
        sprite.sprite = on;
    }

    public void deactivate()
    {
        this.active = false;
        SpriteRenderer sprite = button.GetComponent<SpriteRenderer>();
        sprite.sprite = off;
    }


    public bool isActive()
    {
        return active;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

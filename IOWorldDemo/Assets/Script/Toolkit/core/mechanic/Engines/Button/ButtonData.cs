using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ButtonData 
{
    public Transform Transform;
    public SpriteRenderer SpriteRenderer;
    public Rigidbody2D Rigidbody2D;
    public BoxCollider2D BoxCollider2D;

    public ButtonData(ButtonClick parent) {
        
        this.Transform = parent.GetComponent<Transform>();
        this.SpriteRenderer = parent.GetComponent<SpriteRenderer>();
        this.Rigidbody2D = parent.GetComponent<Rigidbody2D>();
        this.BoxCollider2D =  parent.GetComponent<BoxCollider2D>();
    }
   
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : Interactable
{
    public Transform holder;
    private bool isGrabbed = false;

    void Start() {

        if(executor == null) {
            return;
        }

        this.holder = executor.GetComponent<CharacterController>().holder;
    }

    public override void Interact() {
        
        if(executor == null) {
            return;
        }

        if(holder == null) {
            this.holder = executor.GetComponent<CharacterController>().holder;
        }

        if (isGrabbed) {
            Release();
        } else {
            Grab();
        }
    }

    private void Grab() {

        if(executor == null) {
            return;
        }

        // Get holder from executor
        holder = executor.GetComponent<CharacterController>().holder;

        isGrabbed = true;
        transform.SetParent(holder);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        this.GetComponent<Rigidbody2D>().isKinematic = true;
        this.GetComponent<Rigidbody2D>().Sleep();
    }

    private void Release() {

        Debug.Log("Release");
        isGrabbed = false;
        holder.DetachChildren();
        this.GetComponent<Rigidbody2D>().isKinematic = false;
        this.GetComponent<Rigidbody2D>().WakeUp();

        // Let go of the object and throw it
        //if character rigidbody is moving, add velocity to the object

        float xForce = 0f;
        float yForce = 0f;

        if(executor.GetComponent<Rigidbody2D>().velocity.x != 0) {
            xForce = executor.GetComponent<Rigidbody2D>().velocity.x;
        }

        if(executor.GetComponent<Rigidbody2D>().velocity.y != 0) {
            yForce = executor.GetComponent<Rigidbody2D>().velocity.y;
        }
        this.GetComponent<Rigidbody2D>().AddForce(new Vector2(xForce, yForce + 3f), ForceMode2D.Impulse);
    }

}

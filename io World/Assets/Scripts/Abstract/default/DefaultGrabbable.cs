using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultGrabbable : Grabbable
{
    public Transform holder;
    public bool isGrabbed = false;

    public override void Interact() {
        if (isGrabbed) {
            Release();
        } else {
            Grab();
        }
    }

    protected override void Grab() {
        isGrabbed = true;
        transform.SetParent(holder);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        this.GetComponent<Rigidbody2D>().isKinematic = true;    
    }

    protected override void Release() {
        isGrabbed = false;
        transform.SetParent(null);
        this.GetComponent<Rigidbody2D>().isKinematic = false;   
    }
}

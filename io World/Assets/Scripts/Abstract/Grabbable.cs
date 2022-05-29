using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : Interactable
{
    public Transform holder;
    private bool isGrabbed = false;

    public override void Interact() {
        if (isGrabbed) {
            Release();
        } else {
            Grab();
        }

        isGrabbed = !isGrabbed;
    }


    private void Grab() {
        isGrabbed = true;
        transform.SetParent(holder);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    private void Release() {
        isGrabbed = false;
        transform.SetParent(null);
    }
}

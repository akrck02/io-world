using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Grabbable : Interactable
{
    public abstract override void Interact();
    protected abstract void Grab();
    protected abstract void Release();
}

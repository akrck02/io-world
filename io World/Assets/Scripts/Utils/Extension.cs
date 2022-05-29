using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extension
{
    public static bool IsInteractable(this RaycastHit2D hit)
    {
        return hit.transform.GetComponent<Interactable>();
    }

    public static void Interact(this RaycastHit2D hit)
    {
        hit.transform.GetComponent<Interactable>().Interact();
    }
}

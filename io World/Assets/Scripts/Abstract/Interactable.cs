using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class Interactable : MonoBehaviour
{
    public abstract void Interact();

    private void Reset() {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().OpenInteactableIcon();
            OnInteractionRangeEnter(other);

        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
             other.GetComponent<PlayerController>().CloseInteactableIcon();
             OnInteractionRangeExit(other);
        }
    }


    private void OnInteractionRangeEnter(Collider2D other) {

    }

    private void OnInteractionRangeExit(Collider2D other) {

    }
}

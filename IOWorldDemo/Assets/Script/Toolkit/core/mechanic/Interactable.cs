using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class Interactable : MonoBehaviour
{

    public GameObject executor;

    public abstract void Interact();

    private void Reset() {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            executor = other.gameObject;
            other.GetComponent<CharacterController>().OpenInteactableIcon();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            executor = null;
            other.GetComponent<CharacterController>().CloseInteactableIcon();
        }
    }
}

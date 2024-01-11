using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.VisualScripting;
// using ParrelSync.NonCore;

public class Interactor : NetworkBehaviour
{

    private bool interact;
    private bool inRange;
    public Interactable interactable;

    void Update()
    {
        if (!IsOwner) return;

        if (interactable && Input.GetKeyDown(KeyCode.E)) 
        {
            interactable.Interact();
            interactable.GetComponentInChildren<Animator>().SetTrigger("Click");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsOwner) return;

        if (other.GetComponent<Interactable>()) interactable = other.GetComponent<Interactable>();

        // ENABLE INTERACT UI
        if (other.GetComponent<Interactable>()) other.transform.GetChild(0).gameObject.SetActive(true);
            
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!IsOwner) return;

        // DISABLE INTERACT UI
        if (other.GetComponent<Interactable>()) other.transform.GetChild(0).gameObject.SetActive(false);
        
        interactable = null;
    }
}

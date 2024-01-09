using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Interactor : NetworkBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsOwner) return;

        if (other.GetComponent<Interactable>())
        {
            other.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!IsOwner) return;

        if (other.GetComponent<Interactable>() && Input.GetKeyDown(KeyCode.E))
        {
            other.GetComponentInChildren<Animator>().SetTrigger("Click");
            Debug.Log(other.name);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!IsOwner) return;

        if (other.GetComponent<Interactable>())
        {
            other.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}

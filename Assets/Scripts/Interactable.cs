using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Animator animator;
    
    [SerializeField]
    private bool NPC, item;

    [SerializeField] 
    private Dialogue dialogue;

    public void Interact()
    {
        if (NPC) InteractNPC();
        if (item) InteractItem();
    }

    private void InteractNPC()
    {
        dialogue.SendDialogue();
    }

    private void InteractItem()
    {
        // pick up item?
    }
}

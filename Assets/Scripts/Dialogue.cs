using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    private DialogueManager dialogueManager;
    [SerializeField] private SpriteRenderer portrait;
    [SerializeField] private new string name;
    [SerializeField] private string[] lines;

    public void LinkManager(DialogueManager dialogueManager) {this.dialogueManager = dialogueManager;}

    public void SendDialogue()
    {
        dialogueManager.SetPortrait(portrait);
        dialogueManager.SetName(name);
        dialogueManager.SetLines(lines);
        dialogueManager.StartDialogue();
    }
}

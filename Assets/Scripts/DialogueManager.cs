using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using Unity.Netcode;
using TMPro;
using System.CodeDom.Compiler;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Xml.Serialization;
using System;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private EventManager eventManager;
    private float transitionTime = 0.5f;
    private SpriteRenderer portrait;
    private new string name;
    private string[] lines;
    private List<int> endIndices, eventIndices, choiceIndices, choices;
    [SerializeField] private TextMeshProUGUI character, text;
    [SerializeField] private Image image;
    private bool inProgress, stopTyping, isTyping, makingChoice, onEnd, onEvent;
    private int currentLine;
    private float typeSpeed = 0.03f;

    private void Update()
    {
        // if (!IsOwner) return;

        if (onEnd)
        {
            if (inProgress && !isTyping && Input.GetKeyDown(KeyCode.E)) StartCoroutine(EndDialogue());
        }

        if (onEvent)
        {
            if (!isTyping && Input.GetKeyDown(KeyCode.E)) {SendEvent(currentLine); onEvent = false;}
        }

        if (!makingChoice)
        {
            if (inProgress && isTyping && Input.GetKeyDown(KeyCode.E)) FinishLine();
            if (inProgress && !isTyping && Input.GetKeyDown(KeyCode.E)) {currentLine++; NextLine();}
        }

        if (makingChoice)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) ChoiceMaker(0);
            if (Input.GetKeyDown(KeyCode.Alpha2)) ChoiceMaker(1);
            if (Input.GetKeyDown(KeyCode.Alpha3)) ChoiceMaker(2);
            if (Input.GetKeyDown(KeyCode.Alpha4)) ChoiceMaker(3);
        }
    }

    public void StartDialogue()
    {
        // ENABLE DIALOGUE SCREEN
        transform.GetChild(0).gameObject.SetActive(true);

        currentLine = 0;

        onEnd = false;

        onEvent = false;

        makingChoice = false;
        
        // Freeze player movement
        transform.parent.GetComponent<PlayerMovement>().enabled = false;

        // FREEZE INTERACTOR
        transform.parent.GetComponent<Interactor>().enabled = false;

        endIndices = EndCheck();
        eventIndices = EventCheck();
        choiceIndices = ChoiceCheck();

        StartCoroutine(PaintPortrait());
        StartCoroutine(TypeName());

        stopTyping = false;
        StartCoroutine(TypeLine(0));
    }

    private void FinishLine()
    {
        stopTyping = true;

        // HANDLING SPECIAL LINES
        if (lines[currentLine].Contains("@")) 
        {
            string newLine = lines[currentLine].Split("@")[1];
            text.text = string.Empty; 
            text.text = newLine;
        }
        else {text.text = string.Empty; text.text = lines[currentLine];}
    }

    private void NextLine()
    {
        // START TYPING IN CASE FINISHLINE CALLED
        stopTyping = false;

        // IS THIS AN END LINE?
        if (endIndices.Contains(currentLine)) onEnd = true;

        // IS THIS AN EVENT LINE?
        if (eventIndices.Contains(currentLine)) onEvent = true;

        // END DUE TO LENGTH
        if (currentLine < lines.Length) StartCoroutine(TypeLine(currentLine));
        else StartCoroutine(EndDialogue());
    }

    IEnumerator PaintPortrait()
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
        yield return new WaitForSeconds(transitionTime);
        image.sprite = portrait.sprite;
        image.color = portrait.color;
    }

    IEnumerator TypeName()
    {
        yield return new WaitForSeconds(transitionTime);
        character.text = name;
    }

    IEnumerator TypeLine(int index)
    {
        isTyping = true;
        text.text = string.Empty;

        // IF FIRST LINE, WAIT FOR ANIMATION
        if (index == 0)
        {
            yield return new WaitForSeconds(transitionTime);
            inProgress = true;
        }

        if (lines[index].Contains("$END"))
        {
            string lastLine = lines[index].Split("@")[1];
            foreach (char c in lastLine.ToCharArray())
            {
                if (stopTyping) break;
                text.text += c;
                yield return new WaitForSeconds(typeSpeed);
            }
        }
        else
        {
            // TYPE LINE
            foreach (char c in lines[index].ToCharArray())
            {
                if (stopTyping) break;
                text.text += c;
                yield return new WaitForSeconds(typeSpeed);
            }
        }
        
        // PRINT CHOICES
        if (choiceIndices.Contains(index + 1)) {choices = ChoiceGiver(index + 1);}

        isTyping = false;
    }

    IEnumerator EndDialogue()
    {
        inProgress = false;

        image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
        character.text = string.Empty;
        text.text = string.Empty;

        // DISABLE DIALOGUE SCREEN
        GetComponentInChildren<Animator>().SetTrigger("End");
        yield return new WaitForSeconds(transitionTime);
        transform.GetChild(0).gameObject.SetActive(false);

        // UNFREEZE PLAYER INPUT
        transform.parent.GetComponent<PlayerMovement>().enabled = true;

        // UNFREEZE INTERACTOR
        transform.parent.GetComponent<Interactor>().enabled = true;
    }

    private List<int> EndCheck()
    {
        List<int> endIndices = new List<int>();
        for (int i = 0; i < lines.Length; i++) if (lines[i].Contains("$END")) endIndices.Add(i);
        return endIndices;
    }

    private List<int> EventCheck()
    {
        List<int> eventIndices = new List<int>();
        for (int i = 0; i < lines.Length; i++) if (lines[i].Contains("$EVENT")) eventIndices.Add(i);
        return eventIndices;
    }

    private void SendEvent(int index)
    {
        int startIndex = lines[index].IndexOf("$EVENT:") + 7;
        int endIndex = lines[index].IndexOf(' ', startIndex);
        string eventName = lines[index].Substring(startIndex, endIndex - startIndex);
        eventManager.StartEvent(eventName);
    }

    private List<int> ChoiceCheck()
    {
        List<int> choiceIndices = new List<int>();
        for (int i = 0; i < lines.Length; i++) if (lines[i].Contains("$GOTO")) choiceIndices.Add(i);
        return choiceIndices;
    }

    private List<int> ChoiceGiver(int index)
    {
        makingChoice = true;

        List<int> choices = new List<int>();
        while (choiceIndices.Contains(index))
        {
            string newLine = lines[index];
            newLine = newLine.Split('@')[1];
            text.text += $"\n{newLine}";
            choices.Add(index);
            index++;
        }

        return choices;
    }

    private void ChoiceMaker(int choice)
    {
        //CHECK FOR INVALID CHOICE
        if (choice >= choices.Count) return;

        // GO TO THE CHOICE LINE, PARSE, $GOTO
        currentLine = lines[choices[choice]][5] - '0';

        stopTyping = false;

        NextLine();
        makingChoice = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // if (!IsOwner) return;

        if (other.GetComponent<Dialogue>()) other.GetComponent<Dialogue>().LinkManager(this);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // if (!IsOwner) return;

        // PORTRAIT
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);

        // NAME
        character.text = string.Empty;

        // TEXT
        stopTyping = true;
        text.text = string.Empty;

        if (transform.GetChild(0).gameObject.activeSelf) StartCoroutine(EndDialogue());
    }

    public void SetPortrait(SpriteRenderer portrait) {this.portrait = portrait;}

    public void SetName(string name) {this.name = name;}

    public void SetLines(string[] lines) {this.lines = lines;}
}

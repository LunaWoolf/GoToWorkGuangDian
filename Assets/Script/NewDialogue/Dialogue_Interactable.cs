using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue_Interactable : Dialogue
{
    public BoxCollider boxTrigger;
    private bool playerIsInTrigger = false;
    bool hasInteract = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsInTrigger = false;
        }
    }

    void Start()
    {
        foreach (DialogueObject dialogueObject in DialoguesObjectList)
        {
            dialogueObject.ResetDialogueObject();
        }
       
        if (boxTrigger == null)
        {
            boxTrigger = GetComponent<BoxCollider>();
        }
    }

    void Update()
    {
        if (playerIsInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            OnInteract();
        }
    }

    void OnInteract()
    {
        StartDialogue();
    }

    // Event Base 
    public override void OnDialogueComplete()
    {
        base.OnDialogueComplete();
        hasInteract = true;
    }
 
}

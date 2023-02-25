using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class LocalDialogueManager : MonoSingleton<LocalDialogueManager>
{

    [Header("Reference")]
    DialogueRunner dialogueRunner;
    // Start is called before the first frame update
    void Start()
    {
        dialogueRunner = GameObject.FindObjectOfType<DialogueRunner>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadDialogue(string startNode)
    {
        ViewManager.instance.LoadConversationView();
        if (dialogueRunner == null) return;
        dialogueRunner.StartDialogue(startNode);
    }
}

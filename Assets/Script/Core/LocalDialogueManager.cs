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

    void Awake()
    {
        var objs = FindObjectsOfType<LocalDialogueManager>();

        if (objs.Length > 1)
        {
            foreach (var v in objs)
            {
                if (v.gameObject != this.gameObject)
                    Destroy(v.gameObject);
            }
        }

        DontDestroyOnLoad(this.gameObject);

    }

    void Start()
    {
        dialogueRunner = GameObject.FindObjectOfType<DialogueRunner>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsDialogueExsist(string startNode)
    {
        if (dialogueRunner == null) return false;
        return dialogueRunner.NodeExists(startNode);
    }

    public void LoadDialogue(string startNode)
    {
        if (ViewManager.instance)
        {
            //ViewManager.instance.LoadConversationView();
            ViewManager.instance.UnloadTipView();
            ViewManager.instance.LoadTipView(TipViewController.TipType.DialogueTip);
        }
           
        if (dialogueRunner == null) return;
        
        if (dialogueRunner.IsDialogueRunning)
        {
            dialogueRunner.Stop();
            FindObjectOfType<LineViewCustom>().DialogueComplete();
            dialogueRunner.ResetDialogue(startNode);
        }
        else
        {
            Debug.Log("local try to load" + startNode);
            dialogueRunner.StartDialogue(startNode);
        }

      
    }
}

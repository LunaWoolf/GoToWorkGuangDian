using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class DialogueObject : MonoBehaviour
{
    //public TextMeshProUGUI tm;
    public bool autoProgress;
    public UnityEvent OnDialogueObjectRunComplete_Event;

    public virtual void ResetDialogueObject()
    {

    }

    public virtual void RunDialogueObject()
    {

    }

    public virtual void OnDialogueObjectRunComplete()
    {

    }

    public virtual void FastForwardToComplete()
    {

    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}

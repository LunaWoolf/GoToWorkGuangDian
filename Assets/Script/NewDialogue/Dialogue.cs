using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Dialogue : MonoBehaviour
{
    public string DialogueID;
    public List<DialogueObject> DialoguesObjectList;
    public int currentDialogueIndex = 0;
    DialogueObject currentDialogueProject;

    public bool isLoadSceneWhenCompleteDialogue;
    public string sceneToBuffer;

    public UnityEvent OnDialogueStart_Event;
    public UnityEvent OnDialogueComplete_Event;
    
    void Start()
    {
        foreach (DialogueObject dialogueObject in DialoguesObjectList)
        {
            dialogueObject.ResetDialogueObject();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void StartDialogue()
    {
      
        OnDialogueStart_Event.Invoke();
        if(isLoadSceneWhenCompleteDialogue)
            ScenesManager.instance.StartBufferingScene(sceneToBuffer);

        if (DialoguesObjectList.Count <= 0)
        {
            OnDialogueComplete();
            return;
        }

        currentDialogueIndex = 0;
        foreach (DialogueObject dialogueObject in DialoguesObjectList)
        {
            dialogueObject.ResetDialogueObject();
        }
        this.gameObject.SetActive(true);
        DialoguesObjectList[currentDialogueIndex].RunDialogueObject();
        DialoguesObjectList[currentDialogueIndex].OnDialogueObjectRunComplete_Event.AddListener(RunNextDialogueObject);
    }

    public virtual void RunNextDialogueObject()
    {
        currentDialogueIndex++;
        if (currentDialogueIndex >= DialoguesObjectList.Count)
        {
            OnDialogueComplete();
            return;
        }

        DialoguesObjectList[currentDialogueIndex].RunDialogueObject();
        DialoguesObjectList[currentDialogueIndex].OnDialogueObjectRunComplete_Event.AddListener(RunNextDialogueObject);
    }

    // Event Base 
    public virtual void OnDialogueComplete()
    {
        Debug.Log("This Dialogue Complete");
        OnDialogueComplete_Event.Invoke();

        if (isLoadSceneWhenCompleteDialogue)
            ScenesManager.instance.LoadBufferedScene();
    }

    public virtual void OnNextInputRecieved()
    {
        currentDialogueProject.FastForwardToComplete();

    }

}

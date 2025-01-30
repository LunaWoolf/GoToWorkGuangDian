using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NewDialogueManager : MonoBehaviour
{
    public List<Dialogue> DialogueList;
    public Dialogue CurrentDialogue;

    // Start is called before the first frame update
    void Start()
    {
        

    }

    public void StartCurrentDialogue()
    {
        if (CurrentDialogue)
            CurrentDialogue.StartDialogue();
    }


    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCurrentDialogue();
            //CurrentDialogue.OnNextInputRecieved();
        }
    }
}

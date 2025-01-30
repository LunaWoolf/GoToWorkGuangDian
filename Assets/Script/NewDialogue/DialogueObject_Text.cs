using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueObject_Text : DialogueObject
{
  
    public TextMeshProUGUI tm;
    public float typingSpeed = 0.05f;

    public enum EnterAnimation
    {
        FadeIn,
        TypeWriter,

    }

    void Awake()
    {
        tm = GetComponent<TextMeshProUGUI>();
    }

    public EnterAnimation enterAnimation;

    public override void ResetDialogueObject()
    {
        if (!tm) return;
        tm.maxVisibleCharacters = 0;
        OnDialogueObjectRunComplete_Event.RemoveAllListeners();
    }


    public override void RunDialogueObject()
    {
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        // Get the total number of characters in the text
        int totalCharacters = tm.text.Length;

        // Start with no visible characters
        tm.maxVisibleCharacters = 0;

        // Reveal characters one by one
        for (int i = 0; i <= totalCharacters; i++)
        {
            tm.maxVisibleCharacters = i;  // Update the number of visible characters
            yield return new WaitForSeconds(typingSpeed);  // Wait before showing the next character
        }
        OnDialogueObjectRunComplete();
    }

    public override void OnDialogueObjectRunComplete()
    {
        OnDialogueObjectRunComplete_Event.Invoke();
       
    }

    public override void FastForwardToComplete()
    {
        StopCoroutine(TypeText());
        tm.maxVisibleCharacters = tm.text.Length;
        OnDialogueObjectRunComplete();
    }


   
    void Update()
    {
        
    }
}

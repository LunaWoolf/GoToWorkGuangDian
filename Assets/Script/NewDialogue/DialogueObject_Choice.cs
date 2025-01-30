using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueObject_Choice : DialogueObject
{
    [System.Serializable]
    public struct OptionStruct
    {
        public Option option;
        public UnityEvent OnOptionSelect;

    }

    public List<OptionStruct> OptionList;
    public int selectedOption = -1;

    public override void ResetDialogueObject()
    {
        int i = 0;
        foreach (OptionStruct o in OptionList)
        {
            o.option.ResetOption();
            o.option.SetOpitionIndex(i);
            o.option.parentDialogueObject = this;
            o.option.gameObject.SetActive(false);
            i++;
        }
    }


    public override void RunDialogueObject()
    {
        StartCoroutine(RevealAllOption());
    }

    IEnumerator RevealAllOption()
    {
        foreach (OptionStruct o in OptionList)
        {
            o.option.gameObject.SetActive(true);
            o.option.RevealOption();
            yield return new WaitForSeconds(0.2f);
        }
    }

    public override void OnDialogueObjectRunComplete()
    {
        OnDialogueObjectRunComplete_Event.Invoke();

    }

    public void OnOptionSelect(int optionIndex)
    {
        selectedOption = optionIndex;
        for (int i = 0; i < OptionList.Count; i++)
        {
            if (i == selectedOption)
            {

            } else
            {
                OptionList[i].option.FadeOutOption();
            }
          
        }

        StartCoroutine(IE_OnOptionSelect());
    }

    IEnumerator IE_OnOptionSelect()
    {
        yield return new WaitForSeconds(2f);

        OptionList[selectedOption].OnOptionSelect.Invoke();

    }



    void Update()
    {

    }
}

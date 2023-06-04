using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionWord : Word
{
    [SerializeField] public List<string> PossibleReviseList = new List<string>();
    int currentReviseIndex = 0;

    
    public override void ReviseWord()
    {
        Debug.Log("Revise word mission");
        bool isRevised = true;
        CancleCircledWord();
        ToggleReviseButton(false, true);
        SetText(PossibleReviseList[currentReviseIndex]);
        currentReviseIndex++;
        if (currentReviseIndex == PossibleReviseList.Count)
            currentReviseIndex = 0;
    }

    public override void CircledWord()
    {
        if (!isCircledable) return;
        circled = true;
        LeanTween.value(gameObject, 0f, 1f, .5f).setOnUpdate((float val) =>
        {
            CircleImage.fillAmount = val;
        });

        ToggleReviseButton(true, true);
    }

    public override void CancleCircledWord()
    {
        if (!isCircledable) return;
        circled = false;
        //circled = false;
        CircleImage.fillAmount = 0;
        ToggleReviseButton(false, true);
        /*LeanTween.value(gameObject, 1f, 0f, .5f).setOnUpdate((float val) =>
        {
            CircleImage.fillAmount = val;
        });*/

        //
    }
}

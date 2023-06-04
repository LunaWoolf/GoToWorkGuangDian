using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipViewController : MonoBehaviour
{
    public enum TipType
    {
        Basic,
        Mission_One,
        DialogueTip,
        InteractTip,
    }

    [SerializeField] GameObject BasicTip;
    [SerializeField] GameObject MissionOneTip;
    [SerializeField] GameObject InteractTip;
    [SerializeField] GameObject DialogueTip;


    public void LoadTip(TipType type)
    {
        switch (type)
        {
            case TipType.Basic:
                BasicTip.SetActive(true);
                break;
            case TipType.Mission_One:
                MissionOneTip.SetActive(true);
                break;
            case TipType.InteractTip:
                InteractTip.SetActive(true);
                break;
            case TipType.DialogueTip:
                DialogueTip.SetActive(true);
                break;


        }
    }

    public void UnloadAllTip()
    {
        if(BasicTip != null) BasicTip.SetActive(false);
        if (MissionOneTip != null) MissionOneTip.SetActive(false);
        if (DialogueTip != null) DialogueTip.SetActive(false);
        if (InteractTip != null) InteractTip.SetActive(false);
    }


    // Start is called before the first frame update
    void Start()
    {
        UnloadAllTip();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

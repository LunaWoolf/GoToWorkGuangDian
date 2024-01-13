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
        WorkTip,
        SayTip,
    }

    [SerializeField] GameObject BasicTip;
    [SerializeField] GameObject MissionOneTip;
    [SerializeField] GameObject InteractTip;
    [SerializeField] GameObject DialogueTip;
    [SerializeField] GameObject WorkTip;
    [SerializeField] GameObject SayTip;

    Animator animator;
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
            case TipType.WorkTip:
                WorkTip.SetActive(true);
                break;
            case TipType.SayTip:
                SayTip.SetActive(true);
                break;

        }
    }

    public void UnloadAllTip()
    {

        if(animator!= null) animator.enabled = true;
        if (BasicTip != null) BasicTip.SetActive(false);
        if (MissionOneTip != null) MissionOneTip.SetActive(false);
        if (DialogueTip != null) DialogueTip.SetActive(false);
        if (InteractTip != null) InteractTip.SetActive(false);
        if(WorkTip != null) WorkTip.SetActive(false);
        if (SayTip != null) SayTip.SetActive(false);
    }


    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        UnloadAllTip();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

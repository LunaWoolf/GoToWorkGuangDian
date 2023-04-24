using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DinnerViewController : MonoBehaviour
{
    [Header("Button Ref")]
    [SerializeField] Button MomButton;
    [SerializeField] Button DadButton;
    [SerializeField] Button SisButton;
    [SerializeField] Button TvButton;
    [SerializeField] Button CatButton;
    [SerializeField] Button FamilyButton;

    [Header("DayScene")]
    [SerializeField] GameObject day1Scene;
    [SerializeField] GameObject day2Scene;
    [SerializeField] GameObject day3Scene;
    [SerializeField] GameObject day4Scene;
    [SerializeField] GameObject day5Scene;
    [SerializeField] GameObject day6Scene;

    [SerializeField] Button SaySomethingButton;


    // Start is called before the first frame update
    void Start()
    {
        MomButton.onClick.AddListener(OnMomButtonCliked);
        DadButton.onClick.AddListener(OnDadButtonCliked);
        SisButton.onClick.AddListener(OnSisButtonCliked);
        FamilyButton.onClick.AddListener(OnFamilyButtonCliked);
        TvButton.onClick.AddListener(OnTvButtonCliked);
        CatButton.onClick.AddListener(OnCatButtonClicked);
        SaySomethingButton.onClick.AddListener(OnSaySomethingButtonClicked);
    }

 

    void OnMomButtonCliked()
    {
        string date = GameManager.instance.GetGameDate(); 

        if (LocalDialogueManager.instance.IsDialogueExsist("d" + date + "_Dad"))
        {
            LocalDialogueManager.instance.LoadDialogue("d" + date + "_Dad");
        }
     
        
    }

    void OnFamilyButtonCliked()
    {
        string date = GameManager.instance.GetGameDate();

        if (LocalDialogueManager.instance.IsDialogueExsist("d" + date + "_Family"))
        {
            LocalDialogueManager.instance.LoadDialogue("d" + date + "_Family");
        }
      
    }



    void OnDadButtonCliked()
    {
        LocalDialogueManager.instance.LoadDialogue("I_Phone");
    }

  
    void OnSisButtonCliked()
    {
        GameManager.instance.GoBackToWork();
    }

    void OnTvButtonCliked()
    {
        GameManager.instance.GoBackToWork();
    }

    void OnCatButtonClicked()
    {

        string date = GameManager.instance.GetGameDate();

        if (LocalDialogueManager.instance.IsDialogueExsist("d" + date + "_Cat"))
        {
            LocalDialogueManager.instance.LoadDialogue("d" + date + "_Cat");
        }
    }

    void OnSaySomethingButtonClicked()
    {
        if (GameManager.instance != null)
            GameManager.instance.GoToAfterwork();
    }

}

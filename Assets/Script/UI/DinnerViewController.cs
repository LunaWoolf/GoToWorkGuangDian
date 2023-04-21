using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DinnerViewController : MonoBehaviour
{
    [Header("")]
    [SerializeField] Button MomButton;
    [SerializeField] Button DadButton;
    [SerializeField] Button SisButton;
    [SerializeField] Button TvButton;
    [SerializeField] Button CatButton;

    [SerializeField] Button SaySomethingButton;


    // Start is called before the first frame update
    void Start()
    {
        MomButton.onClick.AddListener(OnMomButtonCliked);
        DadButton.onClick.AddListener(OnDadButtonCliked);
        SisButton.onClick.AddListener(OnSisButtonCliked);
        TvButton.onClick.AddListener(OnTvButtonCliked);
        CatButton.onClick.AddListener(OnCatButtonClicked);
        SaySomethingButton.onClick.AddListener(OnSaySomethingButtonClicked);
    }

 

    void OnMomButtonCliked()
    {
       
        LocalDialogueManager.instance.LoadDialogue("I_Mirror"); // I stand for interactable 
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
       
        GameManager.instance.GoBackToWork();
    }

    void OnSaySomethingButtonClicked()
    {
        Debug.Log("Clicked");
        if (GameManager.instance != null)
            GameManager.instance.GoToAfterwork();
    }

}

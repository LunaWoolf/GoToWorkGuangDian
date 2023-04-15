using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DinnerViewController : MonoSingleton<DinnerViewController>
{
    [Header("")]
    [SerializeField] Button MomButton;
    [SerializeField] Button DadButton;
    [SerializeField] Button SisButton;
    [SerializeField] Button TvButton;
    [SerializeField] Button CatButton;


    // Start is called before the first frame update
    void Start()
    {
      
    }

    private void OnEnable()
    {
        MomButton.onClick.AddListener(OnMomButtonCliked);
        DadButton.onClick.AddListener(OnDadButtonCliked);
        SisButton.onClick.AddListener(OnSisButtonCliked);
        TvButton.onClick.AddListener(OnTvButtonCliked);
        CatButton.onClick.AddListener(OnCatButtonClicked);
    }

    private void OnDisable()
    {
        MomButton.onClick.RemoveListener(OnMomButtonCliked);
        DadButton.onClick.RemoveListener(OnDadButtonCliked);
        SisButton.onClick.RemoveListener(OnSisButtonCliked);
        TvButton.onClick.RemoveListener(OnTvButtonCliked);
        CatButton.onClick.RemoveListener(OnCatButtonClicked);
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
 

}

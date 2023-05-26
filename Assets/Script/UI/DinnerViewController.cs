using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static TVButton;

public class DinnerViewController : MonoBehaviour
{
    [Header("Button Ref")]
    [SerializeField] Button MomButton;
    [SerializeField] Button DadButton;
    [SerializeField] Button SisButton;
    [SerializeField] Button CatButton;
    [SerializeField] Button FamilyButton;
    [SerializeField] Button LakeButton;

    [SerializeField] TVButton TvButton;

    [Header("DayScene")]
    [SerializeField] GameObject day1Scene;
    [SerializeField] GameObject day2Scene;
    [SerializeField] GameObject day3Scene;
    [SerializeField] GameObject day4Scene;
    [SerializeField] GameObject day5Scene;
    [SerializeField] GameObject day6Scene;

    [SerializeField] Button SaySomethingButton;


    bool isNeedToEat = false;
    bool isNeedToWatchTV = false;
    bool isNeedToReviseTV= false;
    bool isNeedToReviseFamily = false;

    bool hasEat = false;
    bool hasWatchTV = false;
    bool hasReviseTV = false;
    bool hasReviseFamily = false;


    // Start is called before the first frame update
    void Start()
    {
     
        //MomButton.onClick.AddListener(OnMomButtonCliked);
       // DadButton.onClick.AddListener(OnDadButtonCliked);
        //SisButton.onClick.AddListener(OnSisButtonCliked);
        //FamilyButton.onClick.AddListener(OnFamilyButtonCliked);
  
        //CatButton.onClick.AddListener(OnCatButtonClicked);
        SaySomethingButton.onClick.AddListener(OnSaySomethingButtonClicked);
        LakeButton.onClick.AddListener(OnLakeButtonClicked);
      
        LakeButton.gameObject.SetActive(false);
        SaySomethingButton.gameObject.SetActive(false);
        SetSceneBaseOnDate();

    }

    public void CheckDinnerStatus()
    {
        if ((isNeedToEat && hasEat) || !isNeedToEat)
        {
            Debug.Log("Eat Pass");
            if ((isNeedToWatchTV && hasWatchTV) || !isNeedToWatchTV)
            {
                Debug.Log("Watch TV Pass");
                if ((isNeedToReviseTV && hasReviseTV) || !isNeedToReviseTV)
                {
                    Debug.Log("Tv Revise pass");
                    if ((isNeedToReviseFamily && hasReviseFamily) || !isNeedToReviseFamily)
                    {
                        Debug.Log("Family Revise pass");
                        CanProgressAfterDinner();

                    }
                }
            }

        }
    }

    void CanProgressAfterDinner()
    {

        if (GameManager.instance.CanEnterLakeMode())
        {
            LakeButton.gameObject.SetActive(true);
        }
        else
        {
            SaySomethingButton.gameObject.SetActive(true);
        }
    }

    public void SetSceneBaseOnDate()
    {
        day1Scene.SetActive(false);
        day2Scene.SetActive(false);
        day3Scene.SetActive(false);
        day4Scene.SetActive(false);
        day5Scene.SetActive(false);
        day6Scene.SetActive(false);

        switch (GameManager.instance.GetDay())
        {
            case 0: // Day1
                day1Scene.SetActive(true);
                TvButton.SetTVState(TVButton.TVState.Default);
                isNeedToEat = true;
                isNeedToWatchTV = true;
                break;
            case 1:// Day2
                day2Scene.SetActive(true);
                TvButton.SetTVState(TVButton.TVState.Default);
                isNeedToEat = true;
                isNeedToWatchTV = true;
                break;
            case 2:// Day3
                day3Scene.SetActive(true);
                TvButton.SetTVState(TVButton.TVState.Broken);
                TvButton.SetIsBroken(true);
                TvButton.SetIsRevisable(false);
                break;
            case 3:// Day4
                day4Scene.SetActive(true);
                TvButton.SetTVState(TVButton.TVState.Text);
                isNeedToReviseTV = true;
                TvButton.SetIsBroken(true);
                TvButton.SetIsRevisable(true);
                break;
            case 4:// Day5
                day5Scene.SetActive(true);
                isNeedToReviseFamily = true;
                TvButton.SetTVState(TVButton.TVState.Text);
                TvButton.SetIsBroken(false);
                TvButton.SetIsRevisable(false);
                foreach (FamilyWordButton fb in FindObjectsOfType<FamilyWordButton>())
                {
                    fb.SetIsBroken(true);
                }
                TvButton.SetIsBroken(false);
                break;
            case 5:
                day6Scene.SetActive(true);
                break;

        }
        CheckDinnerStatus();
    }


    void OnMomButtonCliked()
    {
       
    }

    void OnFamilyButtonCliked()
    {
    
    }

    public void OnFoodButtonClicked()
    {
        hasEat = true;
        CheckDinnerStatus();
    }

    public void OnCloseTV()
    {
        hasWatchTV = true;
        CheckDinnerStatus();
    }

    public void OnReviseTV()
    {
        hasReviseTV = true;
        CheckDinnerStatus();
    }

    public void OnReviseFamily()
    {
        hasReviseFamily = true;
        CheckDinnerStatus();
    }


    void OnLakeButtonClicked()
    {
        if (GameManager.instance != null)
            GameManager.instance.GoToLake();
    }

    void OnDadButtonCliked()
    {
        
    }

  
    void OnSisButtonCliked()
    {
        
    }

    public void OnTvButtonCliked()
    {
        FindObjectOfType<TVManager>().TryLoadTVProgarm();

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
            GameManager.instance.GoToSaySomething();
    }

}

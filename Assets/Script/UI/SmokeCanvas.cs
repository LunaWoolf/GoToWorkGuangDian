using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn;
using TMPro;

public class SmokeCanvas : MonoBehaviour
{

    public Button BuyButton;
    public Button SmokeButton;
    public Button SleepButton;

    // Start is called before the first frame update
    void Start()
    {
        BuyButton.onClick.AddListener(OnBuyButtonClicked);
        SmokeButton.onClick.AddListener(OnSmokeButtonClicked);
        SleepButton.onClick.AddListener(OnSleepButtonClicked);
        SleepButton.interactable = true;
        UpdateCigratteCount();

    }

    void UpdateCigratteCount()
    {
        if (PropertyManager.instance.cigaretteCount > 0)
            SmokeButton.gameObject.SetActive(true);
        else
            SmokeButton.gameObject.SetActive(false);

    }
    void OnBuyButtonClicked()
    {
        if (!GameManager.instance.BuyCigarette())
        {
            ViewManager.instance.LoadTutorialView("You don't have enough money. It take 5 dollor to clear your mind.");
        }

        UpdateCigratteCount();

    }

    void OnSmokeButtonClicked()
    {
        if (PropertyManager.instance.cigaretteCount > 0)
        {
            FindObjectOfType<SaySomethingManager>().Smoke();
          
        }

        UpdateCigratteCount();

    }

    void OnSleepButtonClicked()
    {
      
        FindObjectOfType<SaySomethingManager>().FinishSaySomething();
        SleepButton.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

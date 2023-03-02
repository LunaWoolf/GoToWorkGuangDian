using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MoyuViewController : MonoSingleton<MoyuViewController>
{
    [Header("")]
    [SerializeField] Button GoBackToWorkButton;
    [SerializeField] Button MirrorButton;
    [SerializeField] Button PhoneButton;


    // Start is called before the first frame update
    void Start()
    {
        MirrorButton.onClick.AddListener(OnMirrorButtonCliked);
        PhoneButton.onClick.AddListener(OnPhoneButtonCliked);
        GoBackToWorkButton.onClick.AddListener(OnGoBackToWorkButtonClicked);
    }

    void OnMirrorButtonCliked()
    {
        GameManager.instance.AdjustAndCheckWorkActionCountOfDay(1);
        LocalDialogueManager.instance.LoadDialogue("I_Mirror"); // I stand for interactable 
    }

    void OnPhoneButtonCliked()
    {
        GameManager.instance.AdjustAndCheckWorkActionCountOfDay(1);
        LocalDialogueManager.instance.LoadDialogue("I_Phone");
    }

    void OnGoBackToWorkButtonClicked()
    {
        GameManager.instance.GoBackToWork();
    }

    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OpenSceneController : MonoBehaviour
{

    [SerializeField] Button StartButton;
    [SerializeField] TMP_InputField NameInputField;

    void Start()
    {
        StartButton.onClick.AddListener(OnStartButtonClicked);
    }


    void OnStartButtonClicked()
    {
        if (NameInputField.text == "")
        {
            ViewManager.instance.LoadTutorialView("Please fill in your name on the name card before you go to work!");
        }
        else
        {
            PropertyManager.instance.player_name = NameInputField.text;
            ScenesManager.instance.UnloadScene("OpenScene");
            //this.gameObject.SetActive(false);
        }


    }
}

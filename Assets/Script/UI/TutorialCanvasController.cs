using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialCanvasController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI instructionText;
    [SerializeField] Button GotItButton;

    [Header("Preset")]
    [SerializeField] GameObject Tutorial_Default;
    [SerializeField] GameObject Tutorial_Work_UI;
    [SerializeField] GameObject Tutorial_News_UI;
    // Start is called before the first frame update
    void Start()
    {
        GotItButton.onClick.AddListener(OnGotItButtonClicked);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetInstruction(string text)
    {
        
        if (instructionText == null) return;
        UnloadAllTutorialInstruction();
        switch (text)
        {
            
            case "Tutorial_Work":
                Tutorial_Work_UI.SetActive(true);
                break;
            case "Tutorial_News":
                Tutorial_News_UI.SetActive(true);
                break;
            default:
                Tutorial_Default.SetActive(true);
                instructionText.text = text;
                break;
        }
        
    }


    public void OnGotItButtonClicked()
    {
        this.gameObject.SetActive(false);
        Tutorial_Default.SetActive(false);
        Tutorial_Work_UI.SetActive(false);
        Tutorial_News_UI.SetActive(false);

        if (GameManager.instance.GetCurrentGameMode() == GameManager.GameMode.Work)
        {
            GameManager.instance.isPauseWorkDayTimer = false;
        }
    }

    public void UnloadAllTutorialInstruction()
    {
        instructionText.text = "";
        Tutorial_Default.SetActive(false);
        Tutorial_Work_UI.SetActive(false);
        Tutorial_News_UI.SetActive(false);
    }


}

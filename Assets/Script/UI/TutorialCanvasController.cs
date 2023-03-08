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
    [SerializeField] GameObject TutorialUI;
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
        switch (text)
        {
            
            case "Tutorial":
                TutorialUI.SetActive(true);
                break;
            default:
                instructionText.text = text;
                break;
        }
        
    }


    public void OnGotItButtonClicked()
    {
        this.gameObject.SetActive(false);
        TutorialUI.SetActive(false);
    }


}

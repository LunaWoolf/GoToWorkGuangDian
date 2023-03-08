using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class FeedbackPageManager : MonoBehaviour
{
    public TMP_InputField feedbackInputField;
    public Button SubmitButton;

    // Start is called before the first frame update
    void Start()
    {
        Directory.CreateDirectory(Application.streamingAssetsPath + "/PlayTest_Feedback/");
        SubmitButton.onClick.AddListener(OnSubmitButtonClicked);
    }

    public void OnSubmitButtonClicked()
    {
        CreateFeedback();
    }

    public void NayButtonClicked()
    {
        //CreateFeedback();
    }

    public void CreateFeedback()
    {
        if (feedbackInputField.text == "")
        {
            return;
        }

        string FileName = System.DateTime.Now.ToString().Replace("/", "_");

        string txtDocumentName = Application.streamingAssetsPath + "/PlayTest_Feedback/" + FileName + ".txt";

        if (!File.Exists(txtDocumentName))
        {
            File.WriteAllText(txtDocumentName, "Playtest Feedback: " + txtDocumentName + "\n");
        }

        File.AppendAllText(txtDocumentName, feedbackInputField.text + "\n");
    }

}

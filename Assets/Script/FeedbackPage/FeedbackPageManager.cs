using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;

public class FeedbackPageManager : MonoBehaviour
{
    public TMP_InputField feedbackInputField;
    public Button SubmitButton;
    public Button NayButton;

    // Start is called before the first frame update
    void Start()
    {
        Directory.CreateDirectory(Application.streamingAssetsPath + "/PlayTest_Feedback/");
        SubmitButton.onClick.AddListener(OnSubmitButtonClicked);
        NayButton.onClick.AddListener(OnNayButtonClicked);
    }

    public void OnSubmitButtonClicked()
    {
        CreateFeedback();
        SceneManager.LoadScene("WorkScene");
    }

    public void OnNayButtonClicked()
    {
        SceneManager.LoadScene("WorkScene");
    }

    public void CreateFeedback()
    {
        if (feedbackInputField.text == "")
        {
            return;
        }

        //string FileName = System.DateTime.Now.ToString().Replace("/", "_");
        //FileName = FileName.Replace(" ", "_");
        //FileName = FileName.Replace(":", "_");
        string FileName = PropertyManager.instance.player_name;

        string txtDocumentName = Application.streamingAssetsPath + "/PlayTest_Feedback/" + FileName + ".txt";

        if (!File.Exists(txtDocumentName))
        {
            File.WriteAllText(txtDocumentName, "Playtest Feedback: " + txtDocumentName + "\n");
        }

        File.AppendAllText(txtDocumentName, feedbackInputField.text + "\n");
    }

}

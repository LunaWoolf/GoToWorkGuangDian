using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using Yarn;
using TMPro;

public class SaySomethingManager : MonoBehaviour
{
    [Header("Word Bank")]
    public TextAsset lineRef;

    [Header("Prefab Reference")]
    public GameObject PoemLine;

    public Button NewLineButton;
    public Button DoneButton;
    public Button SleepButton;

    public SmokeCanvas SmokeCanvas;

    public List<string> temp_UsedWordForCurrentLine = new List<string>();
   

    string[] lines;
    // Start is called before the first frame update
    void Start()
    {
        //optimization: don't need to parse it every Time
        if (lineRef != null)
            lines = lineRef.text.Split("\n");

        GenerateLine();

        if(NewLineButton) NewLineButton.onClick.AddListener(OnNewLineButtonClicked);
        if (DoneButton) DoneButton.onClick.AddListener(OnDoneButtonClicked);
        /*if (SleepButton)
        {
            //SleepButton.onClick.AddListener(OnSleepButtonClicked);
            SleepButton.interactable = true;

        } */

        if(SmokeCanvas == null) SmokeCanvas = FindObjectOfType<SmokeCanvas>();
        SmokeCanvas.gameObject.SetActive(false);

        ViewManager.instance.LoadTutorialView("Drag and Drop Words On To  the Paper To Write");
    }

    void OnNewLineButtonClicked()
    {
        if (temp_UsedWordForCurrentLine.Count != 0)
        {
            FindObjectOfType<PaperShredderManager>().StartPaperShredderWithGivenList(temp_UsedWordForCurrentLine);
        }
        GenerateLine();
        temp_UsedWordForCurrentLine.Clear();
    }


    void OnDoneButtonClicked()
    {
        SaveLine();
        if(SmokeCanvas) SmokeCanvas.gameObject.SetActive(true);
        this.GetComponent<Canvas>().enabled = false;
        temp_UsedWordForCurrentLine.Clear();

    }

    public void ReplaceText(Word word, string text)
    {
        if (word.currentWordType == Word.WordType.Empty)
        {
            word.SetText(text);
            word.SetWordType(Word.WordType.Inserted);
        }
        else if (word.currentWordType == Word.WordType.Inserted)
        {
            List<string> temp = new List<string>();
            temp.Add(word.GetCleanText());
            FindObjectOfType<PaperShredderManager>().StartPaperShredderWithGivenList(temp);
            temp_UsedWordForCurrentLine.Remove(word.GetCleanText());
            word.SetText(text);
        }
       
        temp_UsedWordForCurrentLine.Add(text);

    }

    public void Smoke()
    {
        PropertyManager.instance.cigaretteCount -= 1;
        SmokeCanvas.gameObject.SetActive(false);
        ViewManager.instance.LoadTutorialView("your mind is clear, you can write more");
        this.GetComponent<Canvas>().enabled = true;
        GenerateLine();

    }

    void SaveLine()
    {
        string line = "";

        foreach (Word w in PoemLine.GetComponent<PoemLine>().wordList)
        {
            line += " " + w.GetUnProcessText();
        }

        PropertyManager.instance.writeLines.Add(line);

    }

    public void FinishSaySomething()
    {
        GameManager.instance.GoToNextWorkDay();
        ScenesManager.instance.UnloadScene("SaySomethingScene");

    }

    void GenerateLine()
    {
        PoemLine.GetComponent<PoemLine>().ClearLine();
        int randLine = Random.Range(0, lines.Length);
        string line_tem = lines[randLine];
        line_tem = ReplacePlaceholderWithSpace(line_tem);
       
        Debug.Log(line_tem);

        PoemLine.GetComponent<PoemLine>().SetLine(line_tem);
        foreach (Word w in PoemLine.GetComponentsInChildren<Word>())
        { 
            w.isCircledable = false;
            w.tm.fontStyle = FontStyles.Bold;
        }
    }



    public string ReplacePlaceholderWithSpace(string line)
    {
        string[] list = Regex.Split(line, " ");
        string result = "";
        if (list.Length > 0)
        {
            foreach (string s in list)
            {
                string r = s;
                if (s.Contains("<"))
                {
                    r = "<>";
                }
                result += r + " ";
            }

            result = result.Substring(0, result.Length - 1);
        }
        else
        {
            return line;
        }

        return result;
    }

   
}

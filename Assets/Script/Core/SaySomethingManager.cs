using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using Yarn;

public class SaySomethingManager : MonoBehaviour
{
    [Header("Word Bank")]
    public TextAsset lineRef;

    [Header("Prefab Reference")]
    public GameObject PoemLine;

    public Button NewLineButton;
    public Button DoneButton;

    string[] lines;
    // Start is called before the first frame update
    void Start()
    {
        //optimization: don't need to parse it every Time
        if (lineRef != null)
            lines = lineRef.text.Split("\n");

        GenerateLine();

        NewLineButton.onClick.AddListener(GenerateLine);
        DoneButton.onClick.AddListener(OnDoneButtonClicked);
    }

    void OnDoneButtonClicked()
    {


    }

    void GenerateLine()
    {
        PoemLine.GetComponent<PoemLine>().ClearLine();
        int randLine = Random.Range(0, lines.Length);
        string line_tem = lines[randLine];
        line_tem = ReplacePlaceholderWithSpace(line_tem);
       
        Debug.Log(line_tem);

        PoemLine.GetComponent<PoemLine>().SetLine(line_tem);
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

    // Update is called once per frame
    void Update()
    {
        
    }
}

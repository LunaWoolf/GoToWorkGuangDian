using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission : MonoBehaviour
{
    [SerializeField] bool isPassed = false;
    bool allConfirmed = false;
    int totalWord;
    int confirmedWord = 0;
    int currentDay;
    [SerializeField] MissionLine[] MissionLineList;
    string[][] missionLines = new string[4][];
    [SerializeField][TextArea(2, 10)] string[] startMissionLine;
    [SerializeField] List<string> GivenWordList = new List<string>();

    

    // Start is called before the first frame update
    void Start()
    {
        ParseAndSetMissionLine();
        currentDay = GameManager.instance.GetDay();
        FindObjectOfType<PaperShredderManager>().StartPaperShredderWithGivenList(GivenWordList);
    }


    public void ReplaceText(Word word, string text)
    {
        if (word.currentWordType == Word.WordType.Empty)
        {
            word.SetText(text);
            word.isConfirm = true;
            word.SetWordType(Word.WordType.Inserted);
        }
        else if (word.currentWordType == Word.WordType.Inserted)
        {
            List<string> temp = new List<string>();
            temp.Add(word.GetCleanText());
            FindObjectOfType<PaperShredderManager>().StartPaperShredderWithGivenList(temp);
            word.SetText(text);
        }
    }


    void ParseAndSetMissionLine()
    {
        missionLines[0] = GameManager.instance.MissionLine[0].Split(" ");
        missionLines[1] = GameManager.instance.MissionLine[1].Split(" ");
        missionLines[2] = GameManager.instance.MissionLine[2].Split(" ");
        missionLines[3] = GameManager.instance.MissionLine[3].Split(" ");

        MissionLineList[0].SetLine(startMissionLine[0]);
        MissionLineList[1].SetLine(startMissionLine[1]);
        MissionLineList[2].SetLine(startMissionLine[2]);
        MissionLineList[3].SetLine(startMissionLine[3]);

        MissionLineList[0].correctMissionWordList = new List<string>(missionLines[0]);
        MissionLineList[1].correctMissionWordList = new List<string>(missionLines[1]);
        MissionLineList[2].correctMissionWordList = new List<string>(missionLines[2]);
        MissionLineList[3].correctMissionWordList = new List<string>(missionLines[3]);

        MissionLineList[0].SetRevisionWord();
        MissionLineList[1].SetRevisionWord();
        MissionLineList[2].SetRevisionWord();
        MissionLineList[3].SetRevisionWord();

        totalWord = startMissionLine[0].Split(" ").Length +
                    startMissionLine[1].Split(" ").Length +
                    startMissionLine[2].Split(" ").Length +
                    startMissionLine[3].Split(" ").Length;
    }

    void Update()
    {
        
        if (Input.GetMouseButton(0) && !isPassed)
        {

            if (confirmedWord >= totalWord && CheckIfMissionAllConfirmed())
            {
                GameManager.instance.OnCompleteRepeatMission();
                foreach (Word w in GetComponentsInChildren<Word>())
                {
                    w.FadeAndDestroy();
                }

                isPassed = true;
                StartCoroutine(DisableAfterDelay());
            }

            
        }
    }

    IEnumerator DisableAfterDelay()
    {
        yield return new WaitForSeconds(4f);
        this.gameObject.SetActive(false);
    }

    public void OnChildWordConfirm()
    {
        confirmedWord++;
        //Debug.Log("Confirm: " + confirmedWord);
       
    }


    public bool CheckIfMissionAllConfirmed()
    {
        for (int i = 0; i < MissionLineList.Length; i++)
        {
            for (int j = 0; j < MissionLineList[i].wordList.Count; j++)
            {
                if (j >= missionLines[i].Length) return false;
                if (missionLines[i][j].ToLower() != MissionLineList[i].wordList[j]._UnProcessText.ToLower())
                {
                    Debug.Log(missionLines[i][j].ToLower() + "  :  " + MissionLineList[i].wordList[j]._UnProcessText.ToLower());
                    return false;

                } 
            }
        }
        return true;
    }
}


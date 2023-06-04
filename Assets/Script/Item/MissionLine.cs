using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionLine : PoemLine
{
    Mission mission;
    public List<string> correctMissionWordList = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        mission = this.transform.parent.gameObject.GetComponent<Mission>();

        //wordList = new List<Word>(GetComponentsInChildren<Word>());
        if (line01 == null) line01 = this.gameObject.transform;
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnWordConfirmed()
    {
        if (mission == null) mission = this.GetComponentInParent<Mission>();
        mission.OnChildWordConfirm();
    }

    public override GameObject insertWord(string word)
    {
        GameObject w = base.insertWord(word);
     
        return w;
    }

    public override void SetLine(string line)
    {
        base.SetLine(line);

        for (int i = 0; i < wordList.Count; i++)
        {
            if(wordList[i].gameObject.GetComponent<MissionWord>())
                if(i < correctMissionWordList.Count)
                    wordList[i].gameObject.GetComponent<MissionWord>().PossibleReviseList.Add(correctMissionWordList[i]);
        }

        
    }



    public void SetRevisionWord()
    {
        for (int i = 0; i < wordList.Count; i++)
        {
            if (wordList[i].gameObject.GetComponent<MissionWord>())
                wordList[i].gameObject.GetComponent<MissionWord>().PossibleReviseList.Add(correctMissionWordList[i]);
        }

    }

}

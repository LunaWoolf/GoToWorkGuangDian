using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poem : MonoBehaviour
{
    public List<PoemLine> poemLines = new List<PoemLine>();
    public int wordConfirmed = 0;
    public int totalWord = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // if the whole poem is check
    public void SetTotalWordCount()
    {
        totalWord = 0;
        foreach (PoemLine line in poemLines)
        {
            totalWord += line.wordCount;
        }

        Debug.Log("Total: " + totalWord);
    }

    public bool CheckifPoemAllChcked()
    {
        foreach (PoemLine line in poemLines)
        {
            if (!line.checkBox.GetIsCheck())
            {
                return false;
            }

        }


        return true;

    }


    public bool CheckifPoemAllConfirmed()
    {
        if (wordConfirmed >= totalWord -2)
            return true;

        return false;
        

        
    }

    int Politic_Count = 0;
    int Violent_Count = 0;
    int Sexual_Count = 0;
    int Unreal_Count = 0;
    int Negative_Count = 0;
 
    public List<Word.WordQuality> CheckPoemQuality()
    {
        List<Word.WordQuality> qualities = new List<Word.WordQuality>();

        foreach (PoemLine line in poemLines)
        {
            foreach (Word w in line.wordList)
            {
                switch (w.GetWordQuality())
                {
                    case Word.WordQuality.Politic:
                        Politic_Count++;
                        break;
                    case Word.WordQuality.Violent:
                        Violent_Count++;
                        break;
                    case Word.WordQuality.Sexual:
                        Sexual_Count++;
                        break;
                    case Word.WordQuality.Unreal:
                        Unreal_Count++;
                        break;
                    case Word.WordQuality.Negative:
                        Negative_Count++;
                        break;

                }
            }
        }

        if (Politic_Count > 1)
            qualities.Add(Word.WordQuality.Politic);
        if (Violent_Count > 1)
            qualities.Add(Word.WordQuality.Violent);
        if (Sexual_Count > 1)
            qualities.Add(Word.WordQuality.Sexual);
        if (Unreal_Count > 1)
            qualities.Add(Word.WordQuality.Unreal);
        if (Negative_Count > 1)
            qualities.Add(Word.WordQuality.Negative);

        if(qualities.Count == 0)
            qualities.Add(Word.WordQuality.Good);

        return qualities;
    }
}

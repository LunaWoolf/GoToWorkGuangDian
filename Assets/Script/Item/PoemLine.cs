using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoemLine : MonoBehaviour
{
    [Header("Reference")]
    public GameObject wordPrefab;
    public Transform line01;
    public Transform line02;
    [SerializeField] public LineCheckBox checkBox;
    public int oneLineMaxLetter = 50;
    int currentLetterCount = 0;

    public int wordCount = 0;
    public List<Word> wordList = new List<Word>();

    [SerializeField]string[] _line;
    //[SerializeField]string _UnProcessLine;

    void Start()
    {
        
    }

    public virtual void SetLine(string line)
    {
        _line = line.Split(" ");

        foreach (string word in _line)
        {
            insertWord(word);
        }

    }


    public virtual GameObject insertWord(string word)
    {
        
        GameObject w;
        if (currentLetterCount < oneLineMaxLetter)
        {
            w = Instantiate(wordPrefab, line01);
            Debug.Log("LetterCount" + currentLetterCount);

        }
        else
        {
            if (!line02.gameObject.activeSelf)
                line02.gameObject.SetActive(true);

            w = Instantiate(wordPrefab, line02);
            
        }
        wordCount++;
      

        if (word.Contains("<v>"))
        {
            word = word.Replace("<v>", "");
            w.GetComponent<Word>().SetWordType(Word.WordType.Verb);
        }
        else if (word.Contains("<n>"))
        {
            word = word.Replace("<n>", "");
            w.GetComponent<Word>().SetWordType(Word.WordType.Noun);
        }
        else if (word.Contains("<adj>"))
        {
            word = word.Replace("<adj>", "");
            w.GetComponent<Word>().SetWordType(Word.WordType.Adj);
        }
        else if (word.Contains("<>"))
        {
            word = word.Replace("<>", "[__________]");
        

            w.GetComponent<Word>().SetWordType(Word.WordType.Empty);
        }
        else
        {
            w.GetComponent<Word>().SetWordType(Word.WordType.None);
        }

        w.GetComponent<Word>().indexInLine = wordList.Count;
        w.GetComponent<Word>().SetText(word);
    
        wordList.Add(w.GetComponent<Word>());

        currentLetterCount += w.GetComponent<Word>().GetCleanText().Length;

       

        return w;
    }

     
    public void removeLastWord()
    {
        if (wordCount > 0)
        {
            GameObject w = wordList[wordList.Count - 1].gameObject;
            wordList.Remove(w.GetComponent<Word>());
            Destroy(w);
            wordCount--;

        }
        
    }

    public void ClearLine()
    {
        foreach (Word w in this.GetComponentsInChildren<Word>())
        {
            Destroy(w.gameObject);
        }
        wordCount = 0;
        currentLetterCount = 0;
    }
}

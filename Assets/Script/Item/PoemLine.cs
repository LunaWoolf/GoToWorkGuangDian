using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoemLine : MonoBehaviour
{
    [Header("Reference")]
    public GameObject wordPrefab;
    public Transform line01;
    public Transform line02;

    public int wordCount = 0;
    List<Word> wordList = new List<Word>();

    [SerializeField]string[] _line;

    void Start()
    {
        
    }

    public void SetLine(string line)
    {
        _line = line.Split(" ");

        foreach (string word in _line)
        {
            insertWord(word);
        }

    }


    public void insertWord(string word)
    {
        
        GameObject w;
        if (wordCount < 10)
        {
            w = Instantiate(wordPrefab, line01);

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
            word = word.Replace("<>", "         .");
        

            w.GetComponent<Word>().SetWordType(Word.WordType.Empty);
        }
        else
        {
            w.GetComponent<Word>().SetWordType(Word.WordType.None);
        }

        w.GetComponent<Word>().SetText(word);
        wordList.Add(w.GetComponent<Word>());
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
    }
}

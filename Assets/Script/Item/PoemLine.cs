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
        if (wordCount < 7)
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

        w.GetComponent<Word>().SetText(word);
        wordList.Add(w.GetComponent<Word>());
    }
}

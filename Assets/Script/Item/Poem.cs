using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poem
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
}

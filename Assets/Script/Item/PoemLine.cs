using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PoemLine : MonoBehaviour
{
    [Header("Reference")]
    public GameObject wordPrefab;
    public GameObject wordPrefab_Expo;
    public Transform line01;
    public Transform line02;
    [SerializeField] public LineCheckBox checkBox;
    public int oneLineMaxLetter = 50;
    int currentLetterCount = 0;
    int currentTypingWordIndex = 0;

    public int wordCount = 0;
    public List<Word> wordList = new List<Word>();

    public UnityEvent OnLineCompleteType;

    [SerializeField]string[] _line;
    public string roughLine = "";
    public virtual void SetLine(string line)
    {
       _line = line.Split(" ");
       
        roughLine = line.Replace(" ", "");

        if (GameManager.instance.GetCurrentAppMode() == GameManager.AppMode.Expo)
        {
            if (GetComponentInParent<PoemPaperController>())
            {
                OnLineCompleteType.AddListener(GetComponentInParent<PoemPaperController>().OnPoemLineFinishTyping);
            }
            OnLineCompleteType.AddListener(PoemGenerator.instance.TypeNextLine);
            TypeWord(_line[0]);

        }
        else 
        {
            foreach (string word in _line)
            {
                insertWord(word);
            }
        }

    }

    /*IEnumerator TypeLine(string line)
    {
        _line = line.Split(" ");
        foreach (string word in _line)
        {
            TypeWord(word);
            
            yield return new WaitForSeconds(0.2f);
        }
      
    }*/
    IEnumerator IE_TypeNextWord()
    {
        currentTypingWordIndex++;
        if (currentTypingWordIndex >= _line.Length)
        {
            OnLineCompleteType.Invoke();
            yield break;
        }
          
        yield return new WaitForSeconds(0.2f);
        TypeWord(_line[currentTypingWordIndex]);
       
    }

    public void TypeNextWord()
    {
        StartCoroutine(IE_TypeNextWord());
    }

    public virtual GameObject TypeWord(string word)
    {
        GameObject w;
        if (currentLetterCount < oneLineMaxLetter)
        {
            w = Instantiate(wordPrefab_Expo, line01);
            Debug.Log("LetterCount" + currentLetterCount);

        }
        else
        {
            if (!line02.gameObject.activeSelf)
                line02.gameObject.SetActive(true);

            w = Instantiate(wordPrefab_Expo, line02);

        }
        wordCount++;
        word = AnalysisWord(word, w);

        w.GetComponent<ExpoWord>().indexInLine = wordList.Count;
        w.GetComponent<ExpoWord>().OnWordCompleteType.AddListener(TypeNextWord);
        w.GetComponent<ExpoWord>().SetText(word, true);

       
        wordList.Add(w.GetComponent<ExpoWord>());

        currentLetterCount += w.GetComponent<ExpoWord>().GetCleanText().Length;

        return w;
    }



    public virtual string AnalysisWord(string word, GameObject w)
    {

        if (word.Contains("<v>"))
        {
            word = word.Replace("<v>", "");
            if(w.GetComponent<Word>())
                w.GetComponent<Word>().SetWordType(Word.WordType.Verb);
            else
                w.GetComponent<ExpoWord>().SetWordType(Word.WordType.Verb);
        }
        else if (word.Contains("<n>"))
        {
            word = word.Replace("<n>", "");
            if (w.GetComponent<Word>())
                w.GetComponent<Word>().SetWordType(Word.WordType.Noun);
            else
                w.GetComponent<ExpoWord>().SetWordType(Word.WordType.Noun);
        }
        else if (word.Contains("<adj>"))
        {
            word = word.Replace("<adj>", "");
            if (w.GetComponent<Word>())
                w.GetComponent<Word>().SetWordType(Word.WordType.Adj);
            else
                w.GetComponent<ExpoWord>().SetWordType(Word.WordType.Adj);
        }
        else if (word.Contains("<>"))
        {
            word = word.Replace("<>", "[__________]");

            if (w.GetComponent<Word>())
                w.GetComponent<Word>().SetWordType(Word.WordType.Empty);
            else
                w.GetComponent<ExpoWord>().SetWordType(Word.WordType.Empty);
        }
        else
        {
            if (w.GetComponent<Word>())
                w.GetComponent<Word>().SetWordType(Word.WordType.None);
            else
                w.GetComponent<ExpoWord>().SetWordType(Word.WordType.None);
        }

        return word;

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

        word = AnalysisWord(word, w);

        w.GetComponent<Word>().indexInLine = wordList.Count;
        w.GetComponent<Word>().SetText(word, false);
    
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

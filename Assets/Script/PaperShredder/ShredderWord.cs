using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShredderWord : MonoBehaviour
{

    [SerializeField] TextMeshPro[] letterRefList;
    public string word;

    // Start is called before the first frame update
    void Start()
    {
        letterRefList = this.GetComponentsInChildren<TextMeshPro>();
    }

    public void SetWord(string _word)
    {
        word = _word;
        for (int i = 0; i < letterRefList.Length; i++)
        {
            if (i < word.Length)
            {
                if (_word[i] == '.' || _word[i] == '?' || _word[i] == '!' || _word[i] == ',') 
                    continue;
                else
                    letterRefList[i].text = _word[i].ToString();
            }
            else
            {
                letterRefList[i].gameObject.SetActive(false);
            }
        }
        
    }
}

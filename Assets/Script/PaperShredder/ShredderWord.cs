using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShredderWord : MonoBehaviour
{

    [SerializeField] TextMeshPro[] letterRefList;

    // Start is called before the first frame update
    void Start()
    {
        letterRefList = this.GetComponentsInChildren<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void SetWord(string word)
    {
        for (int i = 0; i < letterRefList.Length; i++)
        {
            if (i < word.Length)
            {
                letterRefList[i].text = word[i].ToString();
            }
            else
            {
                letterRefList[i].gameObject.SetActive(false);
            }
        }
        
    }
}

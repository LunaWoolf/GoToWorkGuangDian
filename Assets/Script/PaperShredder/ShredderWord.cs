using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Drawing;

public class ShredderWord : MonoBehaviour
{

    [SerializeField] TextMeshPro[] letterRefList;
    public string word;

    // Start is called before the first frame update
    void Start()
    {
        letterRefList = this.GetComponentsInChildren<TextMeshPro>();
    }

    void Update()
    {
        //check if all finish moving 
        foreach (Rigidbody2D rd in this.GetComponentsInChildren<Rigidbody2D>())
        {
            if (rd.velocity.x > 0 || rd.velocity.y > 0)
            {
                return;
            }
        }

        
    }

    public float GetArea()
    {
        float letterWidth = letterRefList[0].gameObject.GetComponent<RectTransform>().rect.width * letterRefList[0].gameObject.GetComponent<RectTransform>().localScale.x;

        float width = Mathf.Clamp(letterWidth * word.Length - ((letterWidth -1) * this.GetComponent<HorizontalLayoutGroup>().spacing), 0f, 8.82f);

        float height = this.gameObject.GetComponent<RectTransform>().rect.height * this.gameObject.GetComponent<RectTransform>().localScale.y;

        Debug.Log("wh: " + width + " " + height);
        // Calculate the area in screen space
        float area = width * height;
        return area;

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

    public void SetWordColor(UnityEngine.Color color)
    {
        for (int i = 0; i < letterRefList.Length; i++)
        {
            letterRefList[i].color = color;
        }

    }

    public void TurnToWhite()
    {



        
        if (LeanTween.tweensRunning < 100)
        {
            UnityEngine.Color a = letterRefList[0].color;
            UnityEngine.Color b = new UnityEngine.Color(1,1,1,0.2f);
            LeanTween.value(gameObject, a, b, 15f)
            .setOnUpdateColor((UnityEngine.Color color) =>
                {
                    for (int i = 0; i < letterRefList.Length; i++)
                    {
                        letterRefList[i].color = color;

                    }
                   
             });

           
        }
        else
        {
            for (int i = 0; i < letterRefList.Length; i++)
            {
               
                    letterRefList[i].color = UnityEngine.Color.white;
               
            }

        }
       
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Word : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]string _Text;
    public Button wordbutton;
    public TextMeshProUGUI tm;
    public Image CircleImage;
    public GameObject Background;

    [Header("Variable")]
    public bool circled = false;

    public bool banned = false;

    void Start()
    {
        tm.text = _Text;
        if (wordbutton == null) wordbutton = this.GetComponentInChildren<Button>();
        wordbutton.onClick.AddListener(OnWordClicked);
    }

    public void SetText(string t)
    {
        if (t[0] == '?')
        {
            banned = true;
            _Text = t.Substring(1, t.Length-1);
            tm.text = _Text;
            if(PropertyManager.instance.hasCATgpt)
            {
                tm.color = new Color(0.83f, 0, 0, 1);
                PropertyManager.instance.rebelliousCount++;
            }
               
        }
        else
        {
            _Text = t; 
            tm.text = _Text;

            if (PropertyManager.instance.hasCATgpt)
            {
                int i = Random.Range(0, 4);
                if (i < 1) // ramdomly picked as banned word
                {
                    banned = true;
                    tm.color = new Color(0.83f, 0, 0, 1);
                    PropertyManager.instance.rebelliousCount++;
                }
            }
          
        }
        
        if(this.gameObject.activeSelf)StartCoroutine(SetCircleSize()); 
    }
    public string GetText(string t) { return _Text = t; }

    void OnWordClicked()
    {
        if (!circled)
            CircledWord();
        else
            CancleCircledWord();

    }

    void CircledWord()
    {
        circled = true;
        Hashtable options = new Hashtable();
        LeanTween.value(gameObject, 0f, 1f, .5f).setOnUpdate((float val) =>
        {
            CircleImage.fillAmount = val;
        });

        if (PropertyManager.instance.hasCATgpt && banned) // has ai and follow ai instruction
        {
            PropertyManager.instance.rebelliousCount--;
        }
        GameManager.instance.CircledWord(_Text);
    }

    void CancleCircledWord()
    {
        circled = false;

        LeanTween.value(gameObject, 1f, 0f, .5f).setOnUpdate((float val) =>
        {
            CircleImage.fillAmount = val;
        });

        if (PropertyManager.instance.hasCATgpt && banned) // has ai and follow ai instruction
        {
            PropertyManager.instance.rebelliousCount++;
        }

        GameManager.instance.CancleCircledWord(_Text);
    }

    public IEnumerator SetCircleSize()
    {
        yield return new WaitForEndOfFrame();
        if (CircleImage != null && Background != null) 
            CircleImage.GetComponent<RectTransform>().sizeDelta = new Vector2(Background.GetComponent<RectTransform>().sizeDelta.x * 1.3f,
                                                                         Background.GetComponent<RectTransform>().sizeDelta.y * 1.3f);
    }
}

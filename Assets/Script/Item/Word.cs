using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Word : MonoBehaviour
{
    [SerializeField]
    public enum WordType
    {
        None,
        Verb,
        Noun,
        Adj,
        Empty,
    }

    [Header("Reference")]
    [SerializeField]string _Text;
    [SerializeField] string _UnProcessText;
    public Button wordbutton;
    public TextMeshProUGUI tm;
    public Image CircleImage;
    public GameObject Background;

    [Header("Variable")]
    public bool isCircledable = true;
    public bool circled = false;
    public bool banned = false;
    public bool isInserable = false;

    public WordType currentWordType;

    public void SetWordType(WordType type)
    {
     
        currentWordType = type;
        switch (type)
        {
            case WordType.Empty:
                if (wordbutton)
                {
                    //wordbutton.GetComponent<Image>().color = new Color(0, 0, 0, 0f);
                }
                isCircledable = false;
                //tm.fontStyle = FontStyles.Underline;
                break;
        }

    }

    void Start()
    {
        if(tm) tm.text = _Text;
        if (wordbutton == null) wordbutton = this.GetComponentInChildren<Button>();
        if (wordbutton != null) wordbutton.onClick.AddListener(OnWordClicked);
        PoemGenerator.instance.OnPoemRevise.AddListener(ReviseWord);
    }

    public void SetText(string t)
    {
        _UnProcessText = t;

        if (t.Length > 2 && t[0] == '?')
        {
            banned = true;
            _Text = t.Substring(1, t.Length-1);
            _Text = _Text.Replace("_", " ");
            tm.text = _Text;
            if(PropertyManager.instance.hasCATgpt || GameManager.instance.isDebug)
            {
                tm.color = new Color(0.83f, 0, 0, 1);
                PropertyManager.instance.rebelliousCount++;
            }
               
        }
        else
        {
            banned = true;
            _Text = t;
            _Text = _Text.Replace("_", " ");
            tm.text = _Text;

            /*if (PropertyManager.instance.hasCATgpt)
            {
                int i = Random.Range(0, 4);
                if (i < 1) // ramdomly picked as banned word
                {
                    banned = true;
                    tm.color = new Color(0.83f, 0, 0, 1);
                    PropertyManager.instance.rebelliousCount++;
                }
            }*/
          
        }
        
        if(this.gameObject.activeSelf)StartCoroutine(SetCircleSize()); 
    }
    public string GetText() { return _Text; }

    public string GetUnProcessText() { return _UnProcessText; }

    void OnWordClicked()
    {
        if (!circled)
            CircledWord();
        else
            CancleCircledWord();

    }

    void CircledWord()
    {
        if (!isCircledable) return;
        circled = true;
        Hashtable options = new Hashtable();
        LeanTween.value(gameObject, 0f, 1f, .5f).setOnUpdate((float val) =>
        {
            CircleImage.fillAmount = val;
        });

        if (banned) // has ai and follow ai instruction
        {
            PropertyManager.instance.rebelliousCount += 1;
        }
        GameManager.instance.CircledWordInCurrentPoem(_Text);
    }

    void CancleCircledWord()
    {
        if (!isCircledable) return;
        circled = false;

        LeanTween.value(gameObject, 1f, 0f, .5f).setOnUpdate((float val) =>
        {
            CircleImage.fillAmount = val;
        });

        if (banned) // has ai and follow ai instruction
        {
            PropertyManager.instance.rebelliousCount -= 1;
        }

        GameManager.instance.CancleCircledWordInCurrentPoem(_Text);
    }

    public IEnumerator SetCircleSize()
    {
        yield return new WaitForEndOfFrame();
        if (CircleImage != null && Background != null) 
            CircleImage.GetComponent<RectTransform>().sizeDelta = new Vector2(Background.GetComponent<RectTransform>().sizeDelta.x * 1.3f,
                                                                         Background.GetComponent<RectTransform>().sizeDelta.y * 1.3f);
    }

    public void ReviseWord()
    {
        if (circled)
        {
            switch (currentWordType)
            {
                case WordType.Noun:
                    GameManager.instance.CancleCircledWordInCurrentPoem(_Text);
                    SetText(PoemGenerator.instance.GetRandomNoun());
                    break;
                case WordType.Verb:
                    GameManager.instance.CancleCircledWordInCurrentPoem(_Text);
                    SetText(PoemGenerator.instance.GetRandomVerb());
                    break;
                case WordType.Adj:
                    GameManager.instance.CancleCircledWordInCurrentPoem(_Text);
                    SetText(PoemGenerator.instance.GetRandomAdj());
                    break;
            }

                

        }
    }
}

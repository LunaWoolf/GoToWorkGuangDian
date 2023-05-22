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
        Inserted,
        Dialouge,
    }

    [Header("Reference")]
    [SerializeField] public string _Text;
    [SerializeField] public string _Text_clean;
    [SerializeField] public string _UnProcessText;
    public Button wordbutton;
    public Button revisebutton;
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
        if (revisebutton)
        {
            revisebutton.onClick.AddListener(OnReviseButtonClicked);
            ToggleReviseButton(false, true);
        }
        if (PoemGenerator.instance) PoemGenerator.instance.OnPoemRevise.AddListener(ReviseWord);
    }

    public virtual void SetText(string t)
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
               
            }
            //PropertyManager.instance.rebelliousCount++;
            PropertyManager.instance.currentPoemBannedWord++;
        }
        else
        {
            banned = true;
            _Text = t;
            _Text = _Text.Replace("_", " ");
            tm.text = _Text;
            tm.color = new Color(0, 0, 0, 1);
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

        _Text_clean = _Text.Replace(".", "");
        _Text_clean = _Text_clean.Replace("?", "");
        _Text_clean = _Text_clean.Replace("!", "");
        _Text_clean = _Text_clean.Replace(",", "");
        _Text_clean = _Text_clean.Replace(".", "");
        _Text_clean = _Text_clean.Replace("\"", "");
        _Text_clean = _Text_clean.Replace(" ", "");
      
        if (this.gameObject.activeSelf)StartCoroutine(SetCircleSize()); 
    }
    public string GetText() { return _Text; }

    public string GetCleanText() { return _Text_clean; }

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

        /*if (banned) // has ai and follow ai instruction
        {
            PropertyManager.instance.currentPoemBannedWord += 1;
          
        }*/
  
        GameManager.instance.CircledWordInCurrentPoem(_Text_clean);

        ToggleReviseButton(true, true);
    }

    // is reviseable is actually not going to work
    public void ToggleReviseButton(bool isOn, bool isReviseable)
    {
        if (revisebutton == null) return;

        if (!isOn)
        {
            revisebutton.gameObject.SetActive(false);

        }
        else
        {
            revisebutton.gameObject.SetActive(true);
            if (isReviseable)
            {
                //revisebutton.enabled = true;
                revisebutton.GetComponentInChildren<TextMeshProUGUI>().text = "Revise";
            }
            else
            {
                revisebutton.enabled = false;
                revisebutton.GetComponentInChildren<TextMeshProUGUI>().text = "Can't Revise";
            }
        }

    }

    void CancleCircledWord()
    {
        if (!isCircledable) return;
        circled = false;

        LeanTween.value(gameObject, 1f, 0f, .5f).setOnUpdate((float val) =>
        {
            CircleImage.fillAmount = val;
        });

        /*if (banned) // has ai and follow ai instruction
        {
            PropertyManager.instance.currentPoemBannedWord -= 1;
        }*/

        GameManager.instance.CancleCircledWordInCurrentPoem(_Text_clean);
        ToggleReviseButton(false, true);
    }

    public IEnumerator SetCircleSize()
    {
        yield return new WaitForEndOfFrame();
        if (CircleImage != null && Background != null) 
            CircleImage.GetComponent<RectTransform>().sizeDelta = new Vector2(Background.GetComponent<RectTransform>().sizeDelta.x * 1.3f,
                                                                         Background.GetComponent<RectTransform>().sizeDelta.y * 1.3f);
    }

    public void OnReviseButtonClicked()
    {
        if (circled)
            ReviseWord();
    }

    public void ReviseWord()
    {
        Debug.Log("Revise word");
        int day = 0;
        day = GameManager.instance.GetDay();
        if (circled)
        {
            if (banned)
            {
                PropertyManager.instance.currentPoemBannedWord--;
            }
            switch (currentWordType)
            {
                case WordType.Noun:
                    GameManager.instance.CancleCircledWordInCurrentPoem(_Text_clean);
                    FindObjectOfType<PaperShredderManager>().readyToSpawnShredderWordList.Add(_Text_clean);
                    SetText(PoemGenerator.instance.GetRandomNoun(day));
                    GameManager.instance.CircledWordInCurrentPoem(_Text_clean);
                    break;
                case WordType.Verb:
                    GameManager.instance.CancleCircledWordInCurrentPoem(_Text_clean);
                    FindObjectOfType<PaperShredderManager>().readyToSpawnShredderWordList.Add(_Text_clean);
                    SetText(PoemGenerator.instance.GetRandomVerb(day));
                    GameManager.instance.CircledWordInCurrentPoem(_Text_clean);
                    break;
                case WordType.Adj:
                    GameManager.instance.CancleCircledWordInCurrentPoem(_Text_clean);
                    FindObjectOfType<PaperShredderManager>().readyToSpawnShredderWordList.Add(_Text_clean);

                    SetText(PoemGenerator.instance.GetRandomAdj(day));
                    GameManager.instance.CircledWordInCurrentPoem(_Text_clean);
                    break;
                default:
                    //displace not reviseable
                    break;
            }

           
               
        }
    }
}

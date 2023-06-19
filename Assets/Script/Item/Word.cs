using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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

    [SerializeField] Color unconfirmColor = Color.grey; // Starting color
    [SerializeField] Color confirmColor = Color.black; // Ending color

    public float minFadeTime = 2f;
    public float maxFadeTime = 3f;

    public int indexInLine = 0;

    [Header("Variable")]
    public bool isCircledable = true;
    public bool _isConfirm = false;
    public bool isConfirm
    {
        get { return _isConfirm; }
        set
        {
            if (_isConfirm != value)
            {
                _isConfirm = value;
                if (isConfirm)
                {
                    OnWordConfirm.Invoke();
                    GameManager.GameMode currentMode = GameManager.instance.GetCurrentGameMode();
                    if (currentMode == GameManager.GameMode.Work)
                    {
                        if (FindObjectOfType<WorkViewController>()) FindObjectOfType<WorkViewController>().OnWordConfirmed();
                    }
                    else if(currentMode == GameManager.GameMode.Conversation)
                    {
                        if (this.GetComponentInParent<MissionLine>()) this.GetComponentInParent<MissionLine>().OnWordConfirmed();
                    }
                    
                    LeanTween.value(this.gameObject, unconfirmColor, confirmColor, 1f).setOnUpdate((Color val) => { if(tm) tm.color = val; });
                  
                    
                }
                else
                {

                    LeanTween.value(gameObject, confirmColor, confirmColor, 1f).setOnUpdate((Color val) => { if (tm) tm.color = val; });
                }
            }
        }

    }
    public UnityEvent OnWordConfirm;
    public bool circled = false;
    public bool banned = false;
    public bool isInserable = false;

    public WordType currentWordType;
    ClickableObject clickableObject;

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

    private void Awake()
    {
        if (_Text.Length > 0 )
            SetText(_Text);
    }

    protected virtual void Start()
    {
        if(!tm) tm = this.GetComponentInChildren<TextMeshProUGUI>();
        if (tm) tm.text = _Text;
        if (wordbutton == null) wordbutton = this.GetComponentInChildren<Button>();
        //if (wordbutton != null) wordbutton.onClick.AddListener(OnWordClicked);
        if (revisebutton)
        {
            revisebutton.onClick.AddListener(OnReviseButtonClicked);
            ToggleReviseButton(false, true);
        }
        if (PoemGenerator.instance) PoemGenerator.instance.OnPoemRevise.AddListener(ReviseWord);

      
        clickableObject = this.GetComponentInChildren<ClickableObject>();
        if (clickableObject != null)
        {
            clickableObject.ButtonRightClick.AddListener(OnWordRightClicked);
            clickableObject.ButtonLeftClick.AddListener(OnWordLeftClicked);
        }
        else
        {

            this.isConfirm = true;
        }

        if (GameManager.instance.GetCurrentGameMode() == GameManager.GameMode.Dinner)
            this.isConfirm = true;

        this.isConfirm = false;
        if(tm)
            tm.color = unconfirmColor;
    }

    public virtual void SetText(string t)
    {
        _UnProcessText = t;

        if (tm.fontStyle == FontStyles.Underline)
        {
            // Remove the underline style
            tm.fontStyle &= ~FontStyles.Underline;
        }

        if (t.Length > 2 && t[0] == '?')
        {
       

            banned = true;
            _Text = t.Substring(1, t.Length - 1);
            _Text = _Text.Replace("_", " ");
            if (indexInLine == 0)
                _Text = CapFirstLetter(_Text);
            tm.text = _Text;
           

            //hightlight for debug
            if (PropertyManager.instance.hasCATgpt || GameManager.instance.isDebug)
            {
                tm.fontStyle = FontStyles.Underline;

            }
            
            //PropertyManager.instance.rebelliousCount++;
            PropertyManager.instance.currentPoemBannedWord++;
        }
        else
        {
            banned = true;
            _Text = t;
            _Text = _Text.Replace("_", " ");
            if (indexInLine == 0)
                _Text = CapFirstLetter(_Text);
            tm.text = _Text;
            tm.color = new Color(0, 0, 0, 1);
         

        }

        _Text_clean = _Text.Replace(".", "");
        _Text_clean = _Text_clean.Replace("?", "");
        _Text_clean = _Text_clean.Replace("!", "");
        _Text_clean = _Text_clean.Replace(",", "");
        _Text_clean = _Text_clean.Replace(".", "");
        _Text_clean = _Text_clean.Replace("\"", "");
        _Text_clean = _Text_clean.Replace(" ", "");

        if (this.gameObject.activeSelf) StartCoroutine(SetCircleSize());

        if (_Text == "" || _Text == " ")
            isConfirm = true;
    }
    public string GetText() { return _Text; }

    public string GetCleanText() { return _Text_clean; }

    public string GetUnProcessText() { return _UnProcessText; }


    public string CapFirstLetter(string word)
    {
        
 
        char[] charArray = word.ToCharArray();
        if (charArray.Length < 1) return word;

        if (charArray[0] == '?' || charArray[0] == '\'')
        {
            if (charArray.Length < 2) return word;
            charArray[1] = char.ToUpper(charArray[1]);
            string result = new string(charArray);
            return result;
        }
        else if (!char.IsUpper(charArray[0]))
        {
            charArray[0] = char.ToUpper(charArray[0]);
            string result = new string(charArray);
            return result;

        }
        return word;

    }

    void OnWordRightClicked()
    {
        if (!circled)
            CircledWord();
        else
            CancleCircledWord();

    }

    void OnWordLeftClicked()
    {
        isConfirm = true;

    }


    public virtual void CircledWord()
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

    public virtual void CancleCircledWord()
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

 

    public virtual void ReviseWord()
    {
        Debug.Log("Revise word");
        int day = 0;
        day = GameManager.instance.GetDay() + 1;
        bool isRevised = true;
        string orginalText = GetCleanText();
        string ReviceTest = "";
        int breakCoutner = 0;
        if (circled)
        {
            if (banned)
            {
                PropertyManager.instance.currentPoemBannedWord--;
            }
            switch (currentWordType)
            {
                case WordType.Noun:
                    ReviceTest = PoemGenerator.instance.GetRandomNoun(day);
                    while (ReviceTest == GetText() || breakCoutner > 5)
                    {
                        ReviceTest = PoemGenerator.instance.GetRandomNoun(day);
                        breakCoutner++;
                    }
                    break;
                case WordType.Verb:
                    ReviceTest = PoemGenerator.instance.GetRandomVerb(day);
                    while (ReviceTest == GetText() || breakCoutner > 5)
                    {
                        ReviceTest = PoemGenerator.instance.GetRandomVerb(day);
                        breakCoutner++;
                    }
                    break;
                case WordType.Adj:
                    ReviceTest = PoemGenerator.instance.GetRandomAdj(day);
                    while (ReviceTest == GetText() || breakCoutner > 5)
                    {
                        ReviceTest = PoemGenerator.instance.GetRandomAdj(day);
                        breakCoutner++;
                    }
                    break;
                default:
                    //displace not reviseable
                    isRevised = false;
                    break;
            }

            if (ReviceTest != "")
            {
                GameManager.instance.CancleCircledWordInCurrentPoem(_Text_clean);
                FindObjectOfType<PaperShredderManager>().readyToSpawnShredderWordList.Add(_Text_clean);

                SetText(ReviceTest);
                GameManager.instance.CircledWordInCurrentPoem(_Text_clean);
            }
            

            ViewManager.instance.OnWordReviced(isRevised, orginalText, GetCleanText());

        }
    }

    public void FadeAndDestroy()
    {


        // Set the initial color to the current color
        Color initialColor = tm.color;

        // Generate a random duration between minFadeTime and maxFadeTime
        float fadeTime = Random.Range(minFadeTime, maxFadeTime);

        // Use LeanTween to fade the color from the initial color to transparent over fadeTime seconds
        if (LeanTween.tweensRunning < 1000)
        {
            LeanTween.value(gameObject, initialColor, new Color(initialColor.r, initialColor.g, initialColor.b, 0.0f), fadeTime)
           .setOnUpdateColor((Color color) =>
           {
               if (tm)
                   tm.color = color;
           })
           .setOnComplete(() =>
           {
               // Destroy the game object when the fade is complete
               if (this.gameObject)
                   Destroy(gameObject);
           });
        }
        else
        {
            if (this.gameObject)
                Destroy(gameObject);
        }
       

    }
}

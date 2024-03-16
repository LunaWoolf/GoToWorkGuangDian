using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.UI;

public class ExpoWord : Word
{
    // Start is called before the first frame update

    int[] sizeOption = { 50, 50, 50, 60, 70 };

    private RectTransform rectTransform;
    private LayoutElement layoutElement;
    public bool WordComeFromSpeech = false;

    void Awake()
    {
        //isCircledable = false;
        
        // Get the RectTransform component
        rectTransform = GetComponent<RectTransform>();

        // Get or add the LayoutElement component
        layoutElement = GetComponent<LayoutElement>();
        if (layoutElement == null)
        {
            layoutElement = gameObject.AddComponent<LayoutElement>();
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        tm.fontSize = sizeOption[Random.Range(0, sizeOption.Length)];
        OnWordCompleteType.AddListener(AutoFade);
    }

    void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!WordComeFromSpeech)
            {
                StopAllCoroutines();
                FadeAndDestroy();
            }
          
        }

    }

    public void ResetExpoWrod()
    {
        this.gameObject.SetActive(false);
        this.gameObject.transform.SetParent(null);
        this.gameObject.GetComponent<RectTransform>().position = new Vector3(0, 0, 0);
        this.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
        _Text = "";
       _Text_clean = "";
        _UnProcessText = "";
        indexInLine = 0;
        isConfirm = false;
        finishTyping = false;
        isFading = false;
        circled = false;
        banned = false;
        isInserable = false;
        isHighlighted = false;
        isRamdonRevising = false;
        WordComeFromSpeech = false;
        tm.text = "placeholder";
        currentWordType = WordType.None;
        StopCoroutine(IE_RandamRevise());
        StopAllCoroutines();
        Button b = Background.GetComponent<Button>();
        ColorBlock cb = b.colors;
        cb.normalColor = new Color(0, 0, 0, 0);
        b.colors = cb;
        Background.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        
        tm.color = Color.white;
        tm.fontSize = sizeOption[Random.Range(0, sizeOption.Length)];

        OnWordCompleteType.AddListener(AutoFade);
    }
    public IEnumerator SetSize()
    {
        yield return new WaitForEndOfFrame();
        layoutElement.minWidth = rectTransform.rect.width;
        layoutElement.preferredWidth = rectTransform.rect.width;

        // Set the minimum and preferred height to the current height
        layoutElement.minHeight = rectTransform.rect.height;
        layoutElement.preferredHeight = rectTransform.rect.height;
        layoutElement.flexibleWidth = 0;
        layoutElement.flexibleHeight = 0;
        layoutElement.ignoreLayout = false;

        LayoutRebuilder.ForceRebuildLayoutImmediate(this.GetComponentInParent<RectTransform>());
    }

    public void AutoFade()
    {
        StartCoroutine(IE_AutoFade());

    }

    public IEnumerator IE_AutoFade()
    {
        float randomTime = Random.RandomRange(6, 8);
        yield return new WaitForSeconds(randomTime);

        FadeAndDestroy();
        
    }
    public override void SetText(string t, bool isTyping)
    {
       
        base.SetText(t, isTyping);

       
        
    }
    public void SetUnconfirmColor(Color c)
    {
        unconfirmColor = c;

    }
    public void SetTextColor()
    {

        if (tm.text.Contains("`"))
        {
            return;
        }
        tm.color = new Color(tm.color.r, tm.color.g, tm.color.b, 1);
    }

    public override void FadeAndDestroy()
    {
        Color initialColor = tm.color;
        isFading = true;


        float fadeTime = 2f;
        if (isHighlighted)
        {
            Button b = Background.GetComponent<Button>();
            LeanTween.value(gameObject, b.colors.normalColor, new Color(0, 0, 0, 0), 1.8f)
            .setOnUpdateColor((Color color) =>
            {

                ColorBlock cb = b.colors;
                cb.normalColor = color;
                b.colors = cb;

                Background.GetComponent<Image>().color = color;

            });

            isHighlighted = false;
        }
          

        StopAllCoroutines();
        if (LeanTween.tweensRunning < 1000)
        {
            LeanTween.value(gameObject, initialColor, new Color(1, 1, 1, 0.0f), fadeTime)
           .setOnUpdateColor((Color color) =>
           {
               if (tm)
                   tm.color = color;
           })
           .setOnComplete(() =>
           {

               if (this.gameObject)
               {


                   Debug.Log("Enqueue " + PoemGenerator.instance.ExpoWordQueue.Count);

                   ResetExpoWrod();
                   PoemGenerator.instance.ExpoWordQueue.Enqueue(this.gameObject);
               


               }
                 
           });
        }
        else
        {
            if (this.gameObject)
            {
                Debug.Log("Enqueue " + PoemGenerator.instance.ExpoWordQueue.Count);
                ResetExpoWrod();
                PoemGenerator.instance.ExpoWordQueue.Enqueue(this.gameObject);
        
            }
             
        }


    }


    public override void ReviseWord()
    {
        Debug.Log("Revise word Expo");
        int day = 0;
        day = GameManager.instance.GetDay() + 1;

        if (GameManager.instance.GetCurrentAppMode() == GameManager.AppMode.Speed || GameManager.instance.GetCurrentAppMode() == GameManager.AppMode.Expo)
        {
            day = Random.Range(1, 6);
        }
        bool isRevised = true;
        string orginalText = GetCleanText();
        string ReviceTest = "";
        int breakCoutner = 0;
        if (circled || GameManager.instance.GetCurrentAppMode() == GameManager.AppMode.Expo)
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

            // if revise success, then trigger paper shreed base on different mode 
            if (ReviceTest != "")
            {
                GameManager.instance.CancleCircledWordInCurrentPoem(_Text_clean);
                SetText(ReviceTest, true);
                GameManager.instance.CircledWordInCurrentPoem(_Text_clean);
                ViewManager.instance.OnWordReviced(isRevised, orginalText, GetCleanText());
            }

        }
    }

}

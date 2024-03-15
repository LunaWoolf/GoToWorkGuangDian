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

    }

    void Update()
    {
        base.Update();

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

    public IEnumerator AutoFade()
    {
        float randomTime = Random.RandomRange(3, 8);
        yield return new WaitForSeconds(randomTime);

        FadeAndDestroy();
        
    }
    public override void SetText(string t, bool isTyping)
    {
        tm.fontSize = sizeOption[Random.Range(0, sizeOption.Length)];
       // StartCoroutine(SetSize()) ;
        base.SetText(t, isTyping);
      //  StartCoroutine(SetSize());
      
       StartCoroutine(AutoFade());
        
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

      
        float fadeTime = Random.Range(minFadeTime, maxFadeTime);

     
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

               if (this.gameObject)
               {
                   Debug.Log("Enqueue " + PoemGenerator.instance.ExpoWordQueue.Count);
                   PoemGenerator.instance.ExpoWordQueue.Enqueue(this.gameObject);
                   this.gameObject.SetActive(false);
                   this.gameObject.transform.SetParent(null);
                   tm.text = "placeholder";


               }
                 
           });
        }
        else
        {
            if (this.gameObject)
            {
                Debug.Log("Enqueue " + PoemGenerator.instance.ExpoWordQueue.Count);
                PoemGenerator.instance.ExpoWordQueue.Enqueue(this.gameObject);
                this.gameObject.SetActive(false);
                this.gameObject.transform.SetParent(null);
                tm.text = "placeholder";
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

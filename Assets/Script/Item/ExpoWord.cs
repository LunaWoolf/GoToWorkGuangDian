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
        
        isConfirm = true;
        
    }
    public override void SetText(string t, bool isTyping)
    {
        tm.fontSize = sizeOption[Random.Range(0, sizeOption.Length)];
       // StartCoroutine(SetSize()) ;
        base.SetText(t, isTyping);
      //  StartCoroutine(SetSize());
      
       StartCoroutine(AutoFade());
        
    }
 
    public void SetTextColor()
    {

        if (tm.text.Contains("`"))
        {
            return;
        }
        tm.color = new Color(tm.color.r, tm.color.g, tm.color.b, 1);
    }

}

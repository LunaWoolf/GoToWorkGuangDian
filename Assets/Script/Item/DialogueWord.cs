using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueWord : Word
{
    [Header("Reference")]
    //[SerializeField] public new string _Text;
    //[SerializeField] public new string _Text_clean;
    //[SerializeField] public new string _UnProcessText;

    //public override float minFadeTime = 2f;
    //public override float maxFadeTime = 3f;

    int[] sizeOption = { 60, 80, 100 };

    private RectTransform rectTransform;
    private LayoutElement layoutElement;


    void Awake()
    {
        isCircledable = false;

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
        if (tm) tm.text = _Text;
        SetWordType(WordType.Dialouge);
        ToggleReviseButton(false, false);
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


    public override void SetText(string t)
    {


        _UnProcessText = t;
        _Text = t;
        tm.fontSize = sizeOption[Random.Range(0, sizeOption.Length)];
        tm.text = _Text;
        if (t.Contains("`"))
        {


            tm.fontSize = 60;
            tm.color = new Color(0, 0, 0, 0);
            return;
        }

        if (this.gameObject.activeSelf) StartCoroutine(SetCircleSize());
        //if (this.gameObject.activeSelf) StartCoroutine(SetSize());
    }
    // Update is called once per frame
    void Update()
    {
      
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

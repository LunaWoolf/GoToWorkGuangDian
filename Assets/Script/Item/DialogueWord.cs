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

    public float minFadeTime = 2f;
    public float maxFadeTime = 3f;

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
        //tm.fontSize = Random.Range(80, 100);
        tm.text = _Text;

        if (this.gameObject.activeSelf) StartCoroutine(SetCircleSize());
        //if (this.gameObject.activeSelf) StartCoroutine(SetSize());
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeAndDestroy()
    {
       

        // Set the initial color to the current color
        Color initialColor = tm.color;

        // Generate a random duration between minFadeTime and maxFadeTime
        float fadeTime = Random.Range(minFadeTime, maxFadeTime);

        // Use LeanTween to fade the color from the initial color to transparent over fadeTime seconds
        LeanTween.value(gameObject, initialColor, new Color(initialColor.r, initialColor.g, initialColor.b, 0.0f), fadeTime)
            .setOnUpdateColor((Color color) =>
            {
                tm.color = color;
            })
            .setOnComplete(() =>
            {
                // Destroy the game object when the fade is complete
                Destroy(gameObject);
            });

    }
}

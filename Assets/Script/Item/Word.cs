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


    void Start()
    {
        tm.text = _Text;
        if (wordbutton == null) wordbutton = this.GetComponentInChildren<Button>();
        wordbutton.onClick.AddListener(OnWordClicked);
    }

    public void SetText(string t) { _Text = t; tm.text = _Text; if(this.gameObject.activeSelf)StartCoroutine(SetCircleSize()); }
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

        GameManager.instance.CircledWord(_Text);
    }

    void CancleCircledWord()
    {
        circled = false;

        LeanTween.value(gameObject, 1f, 0f, .5f).setOnUpdate((float val) =>
        {
            CircleImage.fillAmount = val;
        });

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

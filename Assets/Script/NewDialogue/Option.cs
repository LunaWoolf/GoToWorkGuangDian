using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems; // Required for Pointer events

public class Option : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public string OptionID;
    int optionIndex;
    public TextMeshProUGUI tm;
    public DialogueObject_Choice parentDialogueObject;
    private float fadeDuration = 1f;
    private Color originalColor;
    public Color hoverColor = Color.yellow; 
    public Color selectedColor = Color.green;

    public void SetOpitionIndex(int i ) { optionIndex = i; }
    public int GetOpitionIndex() { return optionIndex; }

    void Awake()
    {
        tm = GetComponent<TextMeshProUGUI>();
        originalColor = tm.color; // Store the original text color
    }

    public void ResetOption()
    {
        if (!tm) return;
    }

    public void RevealOption()
    {
        StartCoroutine(FadeIn());
    }

    public void FadeOutOption()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        Color textColor = tm.color;
        float alpha = 1f;
        while (alpha > 0f)
        {
            alpha -= Time.deltaTime / fadeDuration;
            textColor.a = Mathf.Clamp01(alpha);
            tm.color = textColor;
            yield return null;
        }

        textColor.a = 0f;
        tm.color = textColor;
    }

    IEnumerator FadeIn()
    {
        Color textColor = tm.color;
        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime / fadeDuration;
            textColor.a = Mathf.Clamp01(alpha);
            tm.color = textColor;
            yield return null;
        }

        textColor.a = 1f;
        tm.color = textColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Change color to hoverColor using LeanTween
        LeanTween.value(gameObject, UpdateTextColor, tm.color, hoverColor, 0.3f).setEase(LeanTweenType.easeInOutQuad);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Change color back to original using LeanTween
        LeanTween.value(gameObject, UpdateTextColor, tm.color, originalColor, 0.3f).setEase(LeanTweenType.easeInOutQuad);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Change color to selectedColor using LeanTween
        LeanTween.value(gameObject, UpdateTextColor, tm.color, selectedColor, 0.3f).setEase(LeanTweenType.easeInOutQuad);
        parentDialogueObject.OnOptionSelect(optionIndex);
      
    }

    private void UpdateTextColor(Color color)
    {
        tm.color = color;
    }
}

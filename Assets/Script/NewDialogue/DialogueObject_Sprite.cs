using System.Collections;
using UnityEngine;

public class DialogueObject_Sprite : DialogueObject
{
    SpriteRenderer spriteRenderer;
    Color spriteColor;
    public float fadeDuration = 2f;

    public enum EnterAnimation
    {
        FadeIn,
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteColor = spriteRenderer.color;
    }

    public EnterAnimation enterAnimation;

    public override void ResetDialogueObject()
    {
        if (!spriteRenderer) return;
        spriteColor.a = 0f;
        spriteRenderer.color = spriteColor;
        OnDialogueObjectRunComplete_Event.RemoveAllListeners();
    }


    public override void RunDialogueObject()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        spriteColor = spriteRenderer.color;
        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime / fadeDuration;  
            spriteColor.a = Mathf.Clamp01(alpha);   
            spriteRenderer.color = spriteColor;     
            yield return null;  
        }

        spriteColor.a = 1f;
        spriteRenderer.color = spriteColor;
        OnDialogueObjectRunComplete();
    }

    public override void OnDialogueObjectRunComplete()
    {
        OnDialogueObjectRunComplete_Event.Invoke();
    }

    public override void FastForwardToComplete()
    {
        StopCoroutine(FadeIn());
        spriteColor.a = 1f;
        spriteRenderer.color = spriteColor;
    }



    void Update()
    {

    }
}

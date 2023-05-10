using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FamilyWordButton : Word
{

    // Start is called before the first frame update
    void Start()
    {
       
        wordbutton.onClick.AddListener(OnWordClicked);
        ToggleReviseButton(false, true);
    }

   
    // Update is called once per frame
    void Update()
    {
        
    }

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

     

       ToggleReviseButton(true, true);
    }


    void CancleCircledWord()
    {
        if (!isCircledable) return;
        circled = false;

        LeanTween.value(gameObject, 1f, 0f, .5f).setOnUpdate((float val) =>
        {
            CircleImage.fillAmount = val;
        });

        ToggleReviseButton(false, true);
    }


}

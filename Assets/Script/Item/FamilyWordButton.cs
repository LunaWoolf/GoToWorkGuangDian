using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FamilyWordButton : Word
{
    bool isReviseable = false;

    ClickableObject _clickableObject;
    DinnerViewController dinnerViewController;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        dinnerViewController = FindObjectOfType<DinnerViewController>();
        _clickableObject = this.GetComponentInChildren<ClickableObject>();
        if (_clickableObject != null)
        {
            _clickableObject.ButtonRightClick.RemoveAllListeners();
            _clickableObject.ButtonLeftClick.RemoveAllListeners();
            _clickableObject.ButtonRightClick.AddListener(dinnerViewController.OnTvButtonCliked);
            _clickableObject.ButtonLeftClick.AddListener(OnWordRightClicked);
        }
      
        ToggleReviseButton(false, true);
    }

    public void SetIsBroken(bool b)
    {
        isReviseable = b;
   
    }

    // Update is called once per frame
    void Update()
    {

        
    }

    void OnWordRightClicked()
    {
        if (!circled && isReviseable)
            CircledWord();
        else if(circled && isReviseable)
            CancleCircledWord();

    }


    public override void ReviseWord()
    {
       

    }


    /*public override void CircledWord()
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


    public override void CancleCircledWord()
    {
        if (!isCircledable) return;
        circled = false;

        LeanTween.value(gameObject, 1f, 0f, .5f).setOnUpdate((float val) =>
        {
            CircleImage.fillAmount = val;
        });

        ToggleReviseButton(false, true);
    }
    */

}

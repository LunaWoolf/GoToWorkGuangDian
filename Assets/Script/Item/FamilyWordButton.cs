using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FamilyWordButton : Word
{
    bool isReviseable = false;

    ClickableObject _clickableObject;
    DinnerViewController dinnerViewController;

    [SerializeField] GameObject[] PossibleReviseList;
    int currentReviseIndex = 0;

    // Start is called before the first frame update
    protected override void Start()
    {
        //base.Start();
        dinnerViewController = FindObjectOfType<DinnerViewController>();
        _clickableObject = this.GetComponentInChildren<ClickableObject>();
        if (_clickableObject != null)
        {
            _clickableObject.ButtonRightClick.RemoveAllListeners();
            _clickableObject.ButtonLeftClick.RemoveAllListeners();
            _clickableObject.ButtonRightClick.AddListener(OnWordRightClicked);
            _clickableObject.ButtonLeftClick.AddListener(OnWordLeftClicked);
        }
      
        ToggleReviseButton(false, true);

        if (revisebutton)
        {
            revisebutton.onClick.AddListener(OnReviseButtonClicked);
            ToggleReviseButton(false, true);
        }

        foreach (GameObject g in PossibleReviseList)
            g.SetActive(false);
    }

    public override void SetText(string t, bool isTyping)
    {

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

    void OnWordLeftClicked()
    {
        

    }

   

    public override void ReviseWord()
    {
        tm.text = "";
        CancleCircledWord();
        wordbutton.enabled = false;
        this.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        PossibleReviseList[currentReviseIndex].SetActive(false);
        currentReviseIndex ++;
        if (currentReviseIndex == PossibleReviseList.Length)
            currentReviseIndex = 0;

        PossibleReviseList[currentReviseIndex].SetActive(true);
        PossibleReviseList[currentReviseIndex].SetActive(true);
        dinnerViewController.OnReviseFamily();
        this.gameObject.SetActive(false);
    }


 
    public override void CancleCircledWord()
    {
        if (!isCircledable) return;
        //circled = false;
        CircleImage.fillAmount = 0;
        /*LeanTween.value(gameObject, 1f, 0f, .5f).setOnUpdate((float val) =>
        {
            CircleImage.fillAmount = val;
        });*/

        //ToggleReviseButton(false, true);
    }
    

}

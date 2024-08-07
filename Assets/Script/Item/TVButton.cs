using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TVButton : Word
{
    bool isBroken = false;
    bool isReviseable = false;
    [SerializeField] GameObject BrokenImage;
    [SerializeField] GameObject RegularImage;
    [Header("UI Reference")]
  
    [SerializeField] Sprite WordVersion;
    [SerializeField] Image TVImage;
    [SerializeField] GameObject TV_Default;
    [SerializeField] GameObject TV_Broken;

    public enum TVState
    {
        Default,
        Broken,
        Text
    }

    TVState state = TVState.Default;

    public TVState GetTVState() { return state; }
    public void SetTVState(TVState s)
    {
        state = s;
        switch (state)
        {
            case TVState.Default:
                TV_Default.SetActive(true);
                TV_Broken.SetActive(false);
                Background.gameObject.SetActive(false);
                tm.gameObject.SetActive(false);
                break;
            case TVState.Broken:
                TV_Default.SetActive(false);
                TV_Broken.SetActive(true);
                Background.gameObject.SetActive(false);
                tm.gameObject.SetActive(false);
                break;
            case TVState.Text:
                TV_Default.SetActive(false);
                TV_Broken.SetActive(false);
                Background.gameObject.SetActive(true);
                tm.gameObject.SetActive(true);
                if (isBroken)
                {
                    BrokenImage.SetActive(true);
                    RegularImage.SetActive(false);
                   //TVImage.sprite = BrokenImage;
                }
                else
                {
                    BrokenImage.SetActive(false);
                    //RegularImage.SetActive(true);
                    //TVImage.sprite = RegularImage;
                }
                break;


        }

    }

    ClickableObject _clickableObject;
    DinnerViewController dinnerViewController;
    // Start is called before the first frame update
    protected override void Start()
    {

        base.Start();
        dinnerViewController = FindObjectOfType<DinnerViewController>();
        this._clickableObject = this.GetComponent<ClickableObject>();
        if (_clickableObject != null)
        {
            this._clickableObject.ButtonRightClick.RemoveAllListeners();
            this._clickableObject.ButtonLeftClick.RemoveAllListeners();
            this._clickableObject.ButtonRightClick.AddListener(OnWordRightClicked);
            this._clickableObject.ButtonLeftClick.AddListener(OnWordLeftClicked);
        }
        SetTVState(state);
    }

    public void SetIsBroken(bool b)
    {
        isBroken = b;
        SetTVState(state);
       
    }

    public void SetIsRevisable(bool b)
    {
        isReviseable = b;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnWordLeftClicked()
    {
        if (!isBroken)
            dinnerViewController.OnTvButtonCliked();


    }

    void OnWordRightClicked()
    {
        Debug.Log("Right Click");
        if (!circled && isBroken && isReviseable)
        {
            CircledWord();

            Debug.Log("Right Click1");
        }
        else if (circled && isBroken)
        {
            CancleCircledWord();
        }
           
    }


    public override void ReviseWord()
    {
        RegularImage.SetActive(true);
        dinnerViewController.OnReviseTV();
        isBroken = false; 
        Debug.Log("Revise TV");
        CancleCircledWord();

    }
}

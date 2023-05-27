using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System;

public class ViewManager : MonoSingleton<ViewManager>
{
    [Header("Art Bank")]
    [SerializeField]  Sprite BossSprite;
    [SerializeField]  Sprite LiSprite;
    [SerializeField]  Sprite CATSprite;
    [SerializeField] Sprite MomSprite;
    [SerializeField] Sprite DadSprite;
    [SerializeField] Sprite SisSprite;
    //[SerializeField]  Sprite YouSprite;

    [Header("Canvas Reference")]
    [SerializeField] GameObject WorkCanvas;
    [SerializeField] GameObject PoemCanvas;
    [SerializeField] GameObject MoyuCanvas;
    [SerializeField] GameObject DialogueCanvas;
    [SerializeField] GameObject TutorialCanvas;
    [SerializeField] GameObject NewsCanvas;
    [SerializeField] GameObject AfterWorkCanvas;
    [SerializeField] GameObject WriteCanvas;
    [SerializeField] GameObject FadeCanvas;
    [SerializeField] GameObject MessageCanvas;
    [SerializeField] GameObject PropertyCanvas;

    [Header("Dialogue Canvas")]
    [SerializeField] Image CharacterImage;

    [SerializeField] TextMeshProUGUI TimerText;

    [SerializeField] public TextMeshProUGUI MoneyText;


    [Header("Door")]
    [SerializeField] Button WorkDoorButton;
    [SerializeField] Button FamilyDoorButton;
    [SerializeField] Canvas DoorCanvas;

    MessageCanvasController messageCanvasController_main;

    void Awake()
    {
        var objs = FindObjectsOfType<ViewManager>();

        if (objs.Length > 1)
        {
            foreach (var v in objs)
            {
                if (v.gameObject != this.gameObject)
                    Destroy(v.gameObject);
            }
        }

        DontDestroyOnLoad(this.gameObject);

    }

    private void Start()
    {
        if (WorkDoorButton)
        {
            WorkDoorButton.onClick.AddListener(OnDoorButtonClicked);

        }
        if (FamilyDoorButton)
        {
            FamilyDoorButton.onClick.AddListener(OnFamilyDoorButtonClicked);

        }

        messageCanvasController_main = FindObjectOfType<MessageCanvasController>();

        ToggleDoorButton(true, true, false);
    }
    public void SetTimerText(float time)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        string formattedTime = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);

        if (TimerText)
            TimerText.text = formattedTime;
    }

    public void SetMoneyText(int money)
    {
        if (MoneyText)
            MoneyText.text = money.ToString();
    }

    public void TogglePropertyCanvas(bool isOn)
    {
        if (PropertyCanvas != null)
            PropertyCanvas.SetActive(isOn);
    }

    public Image GetCharacterImage() { if (CharacterImage == null) CharacterImage = GameObject.Find("CharacterImage").GetComponent<Image>(); return CharacterImage; }
    public MessageCanvasController GetMessageCanvas() { return FindObjectOfType<MessageCanvasController>(); }
    public void SwitchDialogueCharacterArt(string c)
    {
        if (c == string.Empty || c == null)
        {
            GetCharacterImage().gameObject.SetActive(false);
            return;
        } 
        string name = c;
        //Debug.Log(name);
       
        name = name.Replace(" ", "");

        GameManager.Character character;
        if (System.Enum.TryParse<GameManager.Character>(name, out character))
        {
            GetCharacterImage().gameObject.SetActive(true);
            switch (character)
            {
                case GameManager.Character.BossHe:
                    GetCharacterImage().sprite = BossSprite;
                    break;
                case GameManager.Character.Li:
                    GetCharacterImage().sprite = LiSprite;
                    break;
                case GameManager.Character.CATgpt:
                    GetCharacterImage().sprite = CATSprite;
                    break;
                case GameManager.Character.You:
                    //GetCharacterImage().sprite = YouSprite;
                    break;
                case GameManager.Character.Mom:
                    GetCharacterImage().sprite = MomSprite;
                    break;
                case GameManager.Character.Dad:
                    GetCharacterImage().sprite = DadSprite;
                    break;
                case GameManager.Character.Sister:
                    GetCharacterImage().sprite = SisSprite;

                    break;
            }
        }
       
    }


    public void UnloadWorkView() { if (WorkCanvas != null) WorkCanvas.SetActive(false); }
    public void UnloadNewsView() { if (NewsCanvas != null) NewsCanvas.SetActive(false); }
    public void UnloadTutorialView() { if (TutorialCanvas != null) TutorialCanvas.SetActive(false); }

    public void UnloadAllView()
    {
        if (WorkCanvas != null)
            WorkCanvas.SetActive(false);
        if (DialogueCanvas != null)
            DialogueCanvas.SetActive(false);
        if (MoyuCanvas != null)
            MoyuCanvas.SetActive(false);
        if(NewsCanvas != null)
            NewsCanvas.SetActive(false);
        if (TutorialCanvas != null)
            TutorialCanvas.SetActive(false);
        if (AfterWorkCanvas != null)
            AfterWorkCanvas.SetActive(false);
        if (WriteCanvas != null)
            WriteCanvas.SetActive(false);
        if (DoorCanvas != null)
            DoorCanvas.gameObject.SetActive(false);


    }

    public void LoadWorkView(){ if (WorkCanvas != null) WorkCanvas.SetActive(true);}

    public void LoadWriteView()  { if (WriteCanvas != null) WriteCanvas.SetActive(true); }
    public void LoadConversationView()
    {
        DialogueCanvas.SetActive(true);
    }

    public void LoadMoyuView()
    {
        MoyuCanvas.SetActive(true);
    }

    public void LoadTutorialView(string s)
    {
        if (TutorialCanvas == null || TutorialCanvas.GetComponent<TutorialCanvasController>() == null) return;
        if (GameManager.instance.GetCurrentGameMode() == GameManager.GameMode.Work)
        {
            GameManager.instance.isPauseWorkDayTimer = true;
        }
        TutorialCanvas.SetActive(true);
        TutorialCanvas.GetComponent<TutorialCanvasController>().SetInstruction(s);
    }


    public void LoadNewsView()
    {
        if (NewsCanvas == null || NewsCanvas.GetComponent<NewsCanvasController>() == null) return;
        NewsCanvas.SetActive(true);
        NewsManager.instance.GeneratreNews();
    }

    public void LoadAfterWorkView()
    {
        if (AfterWorkCanvas == null) return;
        AfterWorkCanvas.SetActive(true);
    }

    public void FadeToBlack()
    { 
        if(FadeCanvas == null) return;
        FadeCanvas.GetComponent<Animator>().SetTrigger("Fade");
    }

    public void FadeBack()
    {
        if (FadeCanvas == null) return;
        FadeCanvas.GetComponent<Animator>().SetTrigger("UnFade");
    }

    public void FadeToBlack_end()
    {
        if (FadeCanvas == null) return;
        FadeCanvas.GetComponent<Animator>().SetTrigger("FadeEnd");
    }

    public void ToggleDoorButton(bool CanvasIsOn, bool WorkDoorIsOn, bool FamilyDoorIsOn)
    {
        if (DoorCanvas == null) return;
        DoorCanvas.gameObject.SetActive(CanvasIsOn);
        WorkDoorButton.gameObject.SetActive(WorkDoorIsOn);
        FamilyDoorButton.gameObject.SetActive(FamilyDoorIsOn);
    }

    void OnDoorButtonClicked()
    {
        Debug.Log("Try Click on Door");
        if (GameManager.instance.GetDay() >= 6) // Last Day
        {
            //Enter Last Day Mode 
        }
        else
        {
            FindObjectOfType<TimelineManager>().PlayTimeline(FindObjectOfType<TimelineManager>().doorOpenTimeline);
            WorkDoorButton.interactable = false;
        
            //Play Open Door Time line
            //Start Dialogue
        }
    }

    void OnFamilyDoorButtonClicked()
    {
        Debug.Log("Try Click on Door");
       
        if(GameManager.instance.GetDay() < 6)
        {
            FindObjectOfType<TimelineManager>().PlayTimeline(FindObjectOfType<TimelineManager>().familyDoorOpenTimeline);
            FamilyDoorButton.interactable = false;

        }
    }

    public void OnWorkDoorOpenFinish()
    {
        Debug.Log("Door open finished");
        GameManager.instance.LoadMorningWorkDayDialogue();
        WorkDoorButton.interactable = true;
        ToggleDoorButton(false,false,false);
        WorkDoorButton.gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
    }

    public void OnFamilyDoorOpenFinish()
    {
        Debug.Log("Family Door open finished");
        GameManager.instance.GoToDinner();
        FamilyDoorButton.interactable = true;
        ToggleDoorButton(false, false, false);
        FamilyDoorButton.gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
    }

    public void OnWordReviced(bool isRevice, string orginalWord, string revicedWord)
    {
        if(messageCanvasController_main == null) messageCanvasController_main = FindObjectOfType<MessageCanvasController>();
        if (messageCanvasController_main == null)
        {
            Debug.Log("No Message Canvas Controller exsist");
            return;
        }
        messageCanvasController_main.GenerateReplyMessageBlock(isRevice, orginalWord, revicedWord);
    }
}

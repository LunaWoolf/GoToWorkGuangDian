using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ViewManager : MonoSingleton<ViewManager>
{
    [Header("Art Bank")]
    [SerializeField]  Sprite BossSprite;
    [SerializeField]  Sprite LiSprite;
    [SerializeField]  Sprite CATSprite;
    [SerializeField]  Sprite YouSprite;

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

    [Header("Dialogue Canvas")]
    [SerializeField] Image CharacterImage;

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
        Debug.Log(name);
       
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
                    GetCharacterImage().sprite = YouSprite;
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

    public void FadeToBlack_end()
    {
        if (FadeCanvas == null) return;
        FadeCanvas.GetComponent<Animator>().SetTrigger("FadeEnd");
    }
}

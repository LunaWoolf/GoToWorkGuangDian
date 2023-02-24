using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewManager : MonoSingleton<ViewManager>
{
    [Header("Art Bank")]
    [SerializeField]  Sprite BossSprite;
    [SerializeField]  Sprite LiSprite;

    [Header("Canvas Reference")]
    [SerializeField] GameObject WorkCanvas;
    [SerializeField] GameObject PoemCanvas;
    [SerializeField] GameObject DialogueCanvas;
    [SerializeField] GameObject TutorialCanvas;

    [Header("Dialogue Canvas")]
    [SerializeField] Image CharacterImage;

    [Header("Work Canvas")]
    [SerializeField] Button PassButton;
    [SerializeField] Button DenyButton;
    [SerializeField] Animator PoemCanvasAnimator;


    public Image GetCharacterImage() { if (CharacterImage == null) CharacterImage = GameObject.Find("CharacterImage").GetComponent<Image>(); return CharacterImage; }
    
    public void SwitchDialogueCharacterArt(string c)
    {
        if (c == string.Empty || c == null) return;
        string name = c;
        Debug.Log(name);
       
        name = name.Replace(" ", "");
        //if (name.Contains(" "))
        //name = c.Replace(" ", "");

        GameManager.Character character;
        if (System.Enum.TryParse<GameManager.Character>(name, out character))
        {

            switch (character)
            {
                case GameManager.Character.BossHe:
                    GetCharacterImage().sprite = BossSprite;
                    break;
                case GameManager.Character.Li:
                    GetCharacterImage().sprite = LiSprite;
                    break;
            }
        }
    }

    
    void Start()
    {

        if (PassButton != null) PassButton.onClick.AddListener(OnPassButtonClicked);
        if (DenyButton != null) DenyButton.onClick.AddListener(OnDenyButtonClicked);
        if (PoemCanvas != null) PoemCanvasAnimator = PoemCanvas.GetComponent<Animator>();
    }


    void OnPassButtonClicked()
    {
        PoemGenerator.instance.NextPoem();

    }

    void OnDenyButtonClicked()
    {
        PoemGenerator.instance.NextPoem();
    }

    public void UnloadAllView()
    {
        WorkCanvas.SetActive(false);
        DialogueCanvas.SetActive(false);
    }

    public void LoadWorkView()
    {
        WorkCanvas.SetActive(true);
    }

    public void LoadConversationView()
    {
        DialogueCanvas.SetActive(true);
    }
}

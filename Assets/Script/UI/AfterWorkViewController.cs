using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AfterWorkViewController : MonoBehaviour
{
    public GameObject AfterworkChoiceView;
    [SerializeField] Button ThinkButton;
    [SerializeField] Button MusicButton;
    [SerializeField] Button ReadNewsButton;
    [SerializeField] Button WriteButton;

    [SerializeField] string writePrompt = "You only have one thing on your mind";


    public TextMeshProUGUI AfterWorkPromptText;

    // Start is called before the first frame update
    void Start()
    {
        ThinkButton.onClick.AddListener(OnThinkButtonClicked);
        MusicButton.onClick.AddListener(OnMusicButtonClicked);
        ReadNewsButton.onClick.AddListener(OnReadNewsButtonClicked);
        WriteButton.onClick.AddListener(OnWriteButtonClicked);

        ConfigureAfterWorkState();
    }

    void OnEnable()
    {
        ConfigureAfterWorkState();
        AfterworkChoiceView.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnThinkButtonClicked()
    {
   
        if (!PropertyManager.instance.bAesthetic)
        {
            LocalDialogueManager.instance.LoadDialogue("TryUnlockListenToMusic");
        }
        else if (PropertyManager.instance.bAesthetic && !PropertyManager.instance.bReflection)
        {
            LocalDialogueManager.instance.LoadDialogue("TryUnlockReflection");
        }
        else if (PropertyManager.instance.bAesthetic && PropertyManager.instance.bReflection && PropertyManager.instance.hasCATgpt)
        {
            LocalDialogueManager.instance.LoadDialogue("TryUnlockWriting");
        }
        else
        {
            LocalDialogueManager.instance.LoadDialogue("NotCare");
        }

        AfterworkChoiceView.SetActive(false);
    }

    void OnMusicButtonClicked()
    {
        LocalDialogueManager.instance.LoadDialogue("ListenToMusic");
        PropertyManager.instance.bHasListenToMusic = true;
        AfterworkChoiceView.SetActive(false);
    }

    void OnReadNewsButtonClicked()
    {
        GameManager.instance.StartNews();
        AfterworkChoiceView.SetActive(false);
    }

    void OnWriteButtonClicked()
    {
        GameManager.instance.StartSaySomething();
        //LocalDialogueManager.instance.LoadDialogue("ThinkAboutLife");
        AfterworkChoiceView.SetActive(false);
    }

    void ConfigureAfterWorkState()
    {
        HideAllButton();
        if (PropertyManager.instance.bCanWrite)
        {
            WriteButton.gameObject.SetActive(true);
            AfterWorkPromptText.text = writePrompt;
            return;
        }

        if (PropertyManager.instance.bCanListenToMusic && !PropertyManager.instance.bHasListenToMusic)
        {
            MusicButton.gameObject.SetActive(true);
        }
        
        if (PropertyManager.instance.bCanReadNews || PropertyManager.instance.bReflection)
        {
            ReadNewsButton.gameObject.SetActive(true);
        }
        
        ThinkButton.gameObject.SetActive(true);

    }

    void HideAllButton()
    {
        ThinkButton.gameObject.SetActive(false);
        MusicButton.gameObject.SetActive(false);
        ReadNewsButton.gameObject.SetActive(false);
        WriteButton.gameObject.SetActive(false);
    }

  
}

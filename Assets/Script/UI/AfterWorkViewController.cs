using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AfterWorkViewController : MonoBehaviour
{
    public GameObject AfterworkChoiceView;
    public Button ThinkButton;

    
    // Start is called before the first frame update
    void Start()
    {
        ThinkButton.onClick.AddListener(OnThinkButtonClicked);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnThinkButtonClicked()
    {
        LocalDialogueManager.instance.LoadDialogue("ThinkAboutLife");
        AfterworkChoiceView.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using Yarn;

public class WorkViewController : MonoBehaviour
{
    [Header("Work Canvas")]
    [SerializeField] Button PassButton;
    [SerializeField] Button DenyButton;
    [SerializeField] Button MoyuButton;
    [SerializeField] Button NewsButton;
    [SerializeField] Button ReviseButton;
    [SerializeField] Animator PoemCanvasAnimator;

    [Header("ActionCount")]
    [SerializeField] GameObject ActionCountParent;
    [SerializeField] GameObject ActionCountPrefab;
    List<GameObject> ActionCountList = new List<GameObject>();

    [SerializeField] GameObject PoemCanvas;

    [SerializeField][TextArea(5,20)] List<string> DailyWorkPrompt;

    [Header("Work Promnt")]
    [HideInInspector] public Poem CurrentPoemOnCanvas = new Poem();
    [SerializeField] TextMeshProUGUI PromptText;

   

    [SerializeField]
    [TextArea(5, 10)]
    List<string> Boss_Confirmations = new List<string>();


    [SerializeField]
    [TextArea(5, 10)]
    List<string> Boss_Deny = new List<string>();


    // Start is called before the first frame update
    void Start()
    {
        if (PassButton != null)
        {
            PassButton.onClick.AddListener(OnPassButtonClicked);
            PassButton.gameObject.SetActive(false);
        }
        if (DenyButton != null)
        {
            DenyButton.onClick.AddListener(OnDenyButtonClicked);
            DenyButton.gameObject.SetActive(false);
        }

        if (MoyuButton != null)
        {
            MoyuButton.onClick.AddListener(GameManager.instance.StartMoyu);
        }

        if (NewsButton != null)
        {
            NewsButton.onClick.AddListener(GameManager.instance.StartNews);
        }
        if (PoemCanvas != null) PoemCanvasAnimator = PoemCanvas.GetComponent<Animator>();

        if (ReviseButton != null)
        {
            ReviseButton.onClick.AddListener(OnReviseButtonClicked);
            DenyButton.gameObject.SetActive(false);
        }
      
        //InitalActionCount(GameManager.instance.MaxWorkActionCountOfDay - GameManager.instance.WorkActionCountOfDay);
        GameManager.instance.onAction.AddListener(OnUseOneAction);
        UpdatePromptText("");

        GameManager.instance.OnPoemPass.AddListener(OnPoemPass);
        GameManager.instance.OnPoemPassFailed.AddListener(OnPoemPassFailed);
    }

    public void OnWordConfirmed()
    {
        CurrentPoemOnCanvas.wordConfirmed++;
        Debug.Log("Confirm: " + CurrentPoemOnCanvas.wordConfirmed);
        if (CurrentPoemOnCanvas.CheckifPoemAllConfirmed())
        {
            PassButton.gameObject.SetActive(true);
        }
        else
        {
           
            PassButton.gameObject.SetActive(false);

        }
    }


    public void OnLineCheck()
    {

        if (CurrentPoemOnCanvas.CheckifPoemAllChcked())
        {
            PassButton.gameObject.SetActive(true);
        } else
        {
            PassButton.gameObject.SetActive(false);
          
        }
    
    }

    public void OnPoemPass()
    {
        int index = Random.Range(0, Boss_Confirmations.Count);
        UpdatePromptText(Boss_Confirmations[index]);
    }

    public void OnPoemPassFailed()
    {
        //Add camera shake
        int index = Random.Range(0, Boss_Deny.Count);
        UpdatePromptText(Boss_Deny[index]);
    }

    public void SetCurrentPoem( Poem p)
    {
        CurrentPoemOnCanvas = p;
       
        Selectable selectable = PassButton.GetComponent<Selectable>();
        if (selectable != null)
        {
            selectable.transition = Selectable.Transition.ColorTint;
        }
        PassButton.gameObject.SetActive(false);
        Debug.Log("Set Current Poem");
    }
    void OnPassButtonClicked()
    {
        GameManager.instance.OnPoemTryPass();
        
    }

    void OnReviseButtonClicked()
    {
        Debug.Log("On Revise Button Clicked");
        PoemGenerator.instance.OnPoemRevise.Invoke();
    }
    void OnDenyButtonClicked()
    {
        GameManager.instance.OnPoemTryDeny();
     
    }

  


    public void SetPassButtonActive(bool isOn)
    {
        PassButton.enabled = isOn;
    }
    public void SetDenyButtonActive(bool isOn)
    {
        DenyButton.enabled = isOn;
    }

    public void SetNewsButtonActive(bool isOn)
    {
        NewsButton.enabled = isOn;
    }
  
    public void InitalActionCount(int action)
    {
        for (int i = 0; i < action; i++)
        {
            ActionCountList.Add(Instantiate(ActionCountPrefab, ActionCountParent.transform, false));
        }
    }

   

    public void OnUseOneAction()
    {
        if (ActionCountList.Count > 0)
        {
            GameObject g = ActionCountList[0];
            ActionCountList.RemoveAt(0);
            Destroy(g);
        }
    }

    //If pass "", will set prompt text back to daily prompt
    public void UpdatePromptText(string t)
    {
        StopCoroutine(UpdatePromptTextToDeafult(2f));
        if (t == "")
        {
            StartTypewriterEffect(PromptText, DailyWorkPrompt[GameManager.instance.GetDay()], 0.01f);
        }
        else
        {
            StartTypewriterEffect(PromptText, t, 0.01f);
            StartCoroutine(UpdatePromptTextToDeafult(20f));
        } 
    }

    IEnumerator UpdatePromptTextToDeafult(float time)
    {
        yield return new WaitForSeconds(time);
        UpdatePromptText("");
    }


    public void StartTypewriterEffect(TextMeshProUGUI tm, string line, float letterDuration)
    {
        StopCoroutine(TypeText(null,null,0));
        tm.text = "";
        StartCoroutine(TypeText(PromptText,line, letterDuration));
    }

    private IEnumerator TypeText(TextMeshProUGUI tm, string line, float letterDuration)
    {
        tm.text = "";

        foreach (char letter in line)
        {
            tm.text += letter;
            yield return new WaitForSeconds(letterDuration);
        }
    }
}

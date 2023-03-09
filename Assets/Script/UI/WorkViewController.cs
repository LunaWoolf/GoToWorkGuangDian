using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class WorkViewController : MonoBehaviour
{
    [Header("Work Canvas")]
    [SerializeField] Button PassButton;
    [SerializeField] Button DenyButton;
    [SerializeField] Button MoyuButton;
    [SerializeField] Button NewsButton;
    [SerializeField] Animator PoemCanvasAnimator;

    [Header("ActionCount")]
    [SerializeField] GameObject ActionCountParent;
    [SerializeField] GameObject ActionCountPrefab;
    List<GameObject> ActionCountList = new List<GameObject>();

    [SerializeField] GameObject PoemCanvas;

   

    // Start is called before the first frame update
    void Start()
    {
        if (PassButton != null)
        {
            PassButton.onClick.AddListener(OnPassButtonClicked);
        }
        if (DenyButton != null)
        {
            DenyButton.onClick.AddListener(OnDenyButtonClicked);
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

        InitalActionCount(GameManager.instance.MaxWorkActionCountOfDay - GameManager.instance.WorkActionCountOfDay);
        GameManager.instance.onAction.AddListener(OnUseOneAction);
    }

    void OnPassButtonClicked()
    {
        GameManager.instance.OnPoemTryPass();
     
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
}

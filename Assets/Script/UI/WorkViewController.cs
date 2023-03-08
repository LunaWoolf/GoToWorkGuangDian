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
}

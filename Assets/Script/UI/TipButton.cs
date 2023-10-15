using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipButton : MonoBehaviour
{
    [SerializeField]Button button;
    [SerializeField] GameObject TipObject;
    [SerializeField] Animator TipCanvasAnimator;
    public bool startOpen;
    // Start is called before the first frame update
    void Start()
    {
        if (button == null) button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClicked);
        if (startOpen)
        {
            TipObject.SetActive(true);
            TipCanvasAnimator.enabled = false;
            TipCanvasAnimator.gameObject.GetComponent<CanvasGroup>().alpha = 1;
        }
    }

    void OnButtonClicked()
    {
        TipObject.SetActive(!TipObject.activeSelf);

        TipCanvasAnimator.enabled = !TipCanvasAnimator.enabled;
       
        TipCanvasAnimator.gameObject.GetComponent<CanvasGroup>().alpha = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

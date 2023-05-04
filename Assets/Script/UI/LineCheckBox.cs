using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class LineCheckBox : MonoBehaviour
{
    bool isCheck = false;
    [SerializeField] GameObject checkBox;
    [SerializeField] GameObject checkMark;
    [SerializeField] Button checkButton;

    public UnityEvent OnChecked;

    // Start is called before the first frame update
    void Start()
    {
        checkButton.onClick.AddListener(OnCheckBoxClicked);
        if (!isCheck)
        {
            checkMark.SetActive(false);
        }

        if (FindObjectOfType<WorkViewController>())
        {
            OnChecked.AddListener(FindObjectOfType<WorkViewController>().OnLineCheck);
        } 
     
    }

    public bool GetIsCheck()
    {
        return isCheck;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
   
    void OnCheckBoxClicked()
    {
        if (isCheck)
        {
            onUncheck();
        }
        else
        {
            onCheck();
        }

        OnChecked.Invoke();
    }

    public void onCheck()
    {
        isCheck = true;
        checkMark.SetActive(true);
    }

    public void onUncheck()
    {
        isCheck = false;
        checkMark.SetActive(false);
    }

    
}

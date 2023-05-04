using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineCheckBox : MonoBehaviour
{
    bool isCheck = false;
    [SerializeField] GameObject checkBox;
    [SerializeField] GameObject checkMark;
    [SerializeField] Button checkButton;

    // Start is called before the first frame update
    void Start()
    {
        checkButton.onClick.AddListener(OnCheckBoxClicked);
        if (!isCheck)
        {
            checkMark.SetActive(false);
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

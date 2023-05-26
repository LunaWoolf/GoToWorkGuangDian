using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission : MonoBehaviour
{
    public bool canPass = false;
    int totalWord;
    int confirmedWord = 0;
    // Start is called before the first frame update
    void Start()
    {
        totalWord = GetComponentsInChildren<Word>().Length;
        foreach (Word w in GetComponentsInChildren<Word>())
        {
            w.OnWordConfirm.AddListener(OnChildWordConfirm);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (canPass)
            {
                GameManager.instance.OnCompleteRepeatMission();
                foreach (Word w in GetComponentsInChildren<Word>())
                {
                    w.FadeAndDestroy();
                }
            }
            canPass = false;
        }
    }

    void OnChildWordConfirm()
    {
        confirmedWord++;
        if (confirmedWord >= totalWord)
            canPass = true;
    }
}

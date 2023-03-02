using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaperShredderManager : MonoBehaviour
{
    [Header("Reference")]
    public GameObject word;
    public GameObject GoBackButton;
    public Button  DenyButton;
    public Button  PassButton;

    public List<string> shredderWordList = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        GoBackButton.SetActive(false);
        shredderWordList = new List<string>(GameManager.instance.personalBannedWordMap.Keys);
        StartCoroutine(InstantiateWord());
        DenyButton.onClick.AddListener(EndPaperShredder);
        PassButton.onClick.AddListener(EndPaperShredder);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator InstantiateWord()
    {

        foreach (string s in shredderWordList)
        {
            Quaternion q = Quaternion.Euler(Random.Range(0.0f, 10.0f), Random.Range(0.0f, 10.0f), Random.Range(0.0f, 50.0f));
            Instantiate(word, transform.position, q).GetComponent<ShredderWord>().SetWord(s);
            yield return new WaitForSeconds(2f);
        }

        GoBackButton.SetActive(true);


    }

    void EndPaperShredder()
    {
        //ScenesManager.Load
        GameManager.instance.GoToNextWorkDay();

    }
}

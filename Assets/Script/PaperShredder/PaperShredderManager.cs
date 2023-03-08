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

    public GameObject Canvas;

    public List<string> shredderWordList = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        GoBackButton.SetActive(false);
        if (GameManager.instance != null && GameManager.instance.personalBannedWordMap.Keys.Count != 0)
        {
            shredderWordList = new List<string>(GameManager.instance.personalBannedWordMap.Keys);
        }
        
        StartCoroutine(InstantiateWord());
        DenyButton.onClick.AddListener(EndPaperShredder);
        PassButton.onClick.AddListener(EndPaperShredder);
    }


    IEnumerator InstantiateWord()
    {

        foreach (string s in shredderWordList)
        {
            if (s == "Default") continue;
            Quaternion q = Quaternion.Euler(Random.Range(0.0f, 10.0f), Random.Range(0.0f, 10.0f), Random.Range(0.0f, 50.0f));

            Instantiate(word, this.transform.position,Quaternion.identity).GetComponent<ShredderWord>().SetWord(s);
          


            yield return new WaitForSeconds(1f);
        }

        yield return new WaitForSeconds(1f);
        GoBackButton.SetActive(true);


    }

    void EndPaperShredder()
    {
      
        GameManager.instance.GoToAfterwork();

    }
}

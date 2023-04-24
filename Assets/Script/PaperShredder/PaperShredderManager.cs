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

    private float screenHeight;
    private float screenWidth;
    private float screenArea;

    // Start is called before the first frame update
    void Start()
    {
        if (GoBackButton != null) GoBackButton.SetActive(false);
        if (GameManager.instance != null && GameManager.instance.isDebug)
        {
            if (GoBackButton != null) GoBackButton.SetActive(true);
        }
     
        if ((GameManager.instance != null && !GameManager.instance.isDebug) && GameManager.instance.personalBannedWordMap.Keys.Count != 0)
        {
            shredderWordList = new List<string>(GameManager.instance.personalBannedWordMap.Keys);
        }
        
        StartCoroutine(InstantiateWord());
        if (DenyButton != null) DenyButton.onClick.AddListener(EndPaperShredder);
        if (PassButton != null) PassButton.onClick.AddListener(EndPaperShredder);

        screenHeight = Camera.main.orthographicSize * 2;
        screenWidth = screenHeight * Screen.width / Screen.height;
        screenArea = screenHeight * screenWidth * 5; // I have no fucking idea why i need to multiply it by 5 here

        Debug.Log(screenHeight + " " + screenWidth + "  " + " " + screenArea);
    }


    IEnumerator InstantiateWord()
    {
        Debug.Log("Start Paper Shredder");
        foreach (string s in shredderWordList)
        {
            if (s == "Default") continue;
            Quaternion q = Quaternion.Euler(Random.Range(0.0f, 10.0f), Random.Range(0.0f, 10.0f), Random.Range(0.0f, 50.0f));

            if (Canvas != null)
            {
                GameObject w = Instantiate(word, Canvas.transform);
                w.GetComponent<ShredderWord>().SetWord(s);
            }
            else
            {
                GameObject w = Instantiate(word, this.transform.position, Quaternion.identity);
                w.GetComponent<ShredderWord>().SetWord(s);
            }

           yield return new WaitForSeconds(.5f);
        }

        yield return new WaitForSeconds(1f);
        if (GoBackButton != null) GoBackButton.SetActive(true);


    }

    void EndPaperShredder()
    {
        GameManager.instance.GoToBus();
        GoBackButton.SetActive(false);
        //GameManager.instance.GoToAfterwork();

    }

    void Update()
    {
        
        //screenArea = Screen.width * Screen.height;

        float totalArea = 0f;

        ShredderWord[] ShredderWords = FindObjectsOfType<ShredderWord>();

        for (int i = 0; i < ShredderWords.Length; i++)
        {

         
            float area = ShredderWords[i].GetArea();

            totalArea += area;
        }

        if ((totalArea / screenArea) * 100 >= 80)
        {
            ScreenFilled();
        }
      
        Debug.Log("Filled Precent: " + (totalArea / screenArea) * 100 + "%");
    }

    void ScreenFilled()
    {
        // Your function to fire when objects have filled up the whole screen goes here
        Debug.Log("Screen filled!");
    }
}

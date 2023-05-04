using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaperShredderManager : MonoBehaviour
{
    [Header("Reference")]
    public GameObject word;
    public GameObject GoBackButton;
    //public Button  DenyButton;
    //public Button  PassButton;
    public Button GoHomeButton;

    public GameObject Canvas;

    public List<string> readyToSpawnShredderWordList = new List<string>();
    List<ShredderWord> SpawnedShredderWordList = new List<ShredderWord>();

    private float screenHeight;
    private float screenWidth;
    private float screenArea;

    [Header("Filled")]
    public float maxFilledPrecent = 80;
    [SerializeField] Color startColor;
    [SerializeField] Color endColor;

    // Start is called before the first frame update
    void Start()
    {
        if (GoBackButton != null) GoBackButton.SetActive(false);
       
        if (GoHomeButton != null) GoHomeButton.onClick.AddListener(EndPaperShredder);
        //if (PassButton != null) PassButton.onClick.AddListener(EndPaperShredder);

        screenHeight = Camera.main.orthographicSize * 2;
        screenWidth = screenHeight * Screen.width / Screen.height;
        screenArea = screenHeight * screenWidth * 5; // I have no fucking idea why i need to multiply it by 5 here

        Debug.Log(screenHeight + " " + screenWidth + "  " + " " + screenArea);
       
    }

    public void StartPaperShredder()
    {

        if (GameManager.instance != null && GameManager.instance.isDebug)
        {
            if (GoBackButton != null) GoBackButton.SetActive(true);
        }

        if (GameManager.instance != null /*&& !GameManager.instance.isDebug)*/ && GameManager.instance.personalBannedWord_Day.Count != 0)
        {
            readyToSpawnShredderWordList.AddRange( GameManager.instance.personalBannedWord_Day);
        }

        StartCoroutine(InstantiateWord());
    }

    public void StartPaperShredderWithGivenList(List<string> strings)
    {
        readyToSpawnShredderWordList.AddRange(strings);
        StartCoroutine(InstantiateWord());
    }

    IEnumerator InstantiateWord()
    {
        Debug.Log("Start Paper Shredder");
        foreach (string s in readyToSpawnShredderWordList)
        {
            if (s == "Default") continue;
            Quaternion q = Quaternion.Euler(Random.Range(0.0f, 10.0f), Random.Range(0.0f, 10.0f), Random.Range(0.0f, 50.0f));

            if (Canvas != null)
            {
                GameObject w = Instantiate(word, Canvas.transform);
                w.GetComponent<ShredderWord>().SetWord(s);
                SpawnedShredderWordList.Add(w.GetComponent<ShredderWord>());
            }
            else
            {
                Vector3  pos = new Vector3(this.transform.position.x + Random.Range(- screenWidth / 2, screenWidth /4), this.transform.position.y, this.transform.position.z);
                GameObject w = Instantiate(word, pos, Quaternion.identity);
              
                w.GetComponent<ShredderWord>().SetWord(s);
                SpawnedShredderWordList.Add(w.GetComponent<ShredderWord>());

            }
           
            CheckWordFilledAmaountAndSetColor();
            yield return new WaitForSeconds(.5f);

            
        }
        Debug.Log("Clear Current read to spawn word list");
        readyToSpawnShredderWordList.Clear();
        yield return new WaitForSeconds(1f);

        if (GameManager.instance.GetCurrentGameMode() == GameManager.GameMode.PaperShredder && GoBackButton != null) GoBackButton.SetActive(true);

    }

    void EndPaperShredder()
    {
        GoBackButton.SetActive(false);
        GameManager.instance.GoToBus();
       
    
        //GameManager.instance.GoToAfterwork();

    }

    void Update()
    {

        //screenArea = Screen.width * Screen.height;
       
    }

    float CheckWordFilledAmaountAndSetColor()
    {
        float totalArea = 0f;

        ShredderWord[] ShredderWords = FindObjectsOfType<ShredderWord>();

        for (int i = 0; i < ShredderWords.Length; i++)
        {
            float area = ShredderWords[i].GetArea();
            totalArea += area;
        }

        float fillPrecent = (totalArea / screenArea) * 100;

        for (int i = 0; i < ShredderWords.Length; i++)
        {
            Color color = Color.Lerp(startColor, endColor, fillPrecent / 100);
            ShredderWords[i].SetWordColor(color);
        }


        if (fillPrecent >= maxFilledPrecent)
        {
            ScreenFilled();
        }

        return fillPrecent;
    }

    void ScreenFilled()
    {
        // Your function to fire when objects have filled up the whole screen goes here
        Debug.Log("Screen filled!");
    }
}

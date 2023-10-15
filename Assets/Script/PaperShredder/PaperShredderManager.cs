using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using URPGlitch;
using URPGlitch.Runtime.AnalogGlitch;
using Kino;

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
    public float maxFilledWord = 10;
    [SerializeField] Color startColor;
    [SerializeField] Color endColor;

    public float negativeGravity = -0.5f;

    public Volume postProcessingVolume; // Reference to your Post Processing Volume
    private VolumeProfile profile; // The profile associated with the Post Processing Volume

    // Vignette and Film Grain parameters
    private Vignette vignette;
    private FilmGrain filmGrain;
    URPGlitch.Runtime.AnalogGlitch.AnalogGlitchVolume AnalogGlitch;

    public Animator ciga_animatorController;
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

        profile = postProcessingVolume.sharedProfile;

        // Check if the profile contains a Vignette and FilmGrain component
        profile.TryGet(out vignette);
        profile.TryGet(out filmGrain);

        
        SetVignetteIntensity(0.2f);
        SetFilmGrainIntensity(0.2f);
        if (AnalogGlitch != null)
            AnalogGlitch.scanLineJitter = new ClampedFloatParameter(0.005f, 0.005f, 0.4f);

        if (GameManager.instance.GetCurrentAppMode() == GameManager.AppMode.Speed && ViewManager.instance.GetWorkView())
        {
            ViewManager.instance.GetWorkView().OnSpeedSmokeStart.AddListener(SpeedSmokeStart);
            ViewManager.instance.GetWorkView().OnSpeedSmokeFinish.AddListener(SpeedSmokeFinish);
        }
           
    }

    public void SpeedSmokeStart()
    {
        ChangeGravity();
    }

    public void SpeedSmokeFinish()
    {
        ChangeGravityBack();
    }

    public void SetVignetteIntensity(float intensity)
    {
        vignette.intensity.value = intensity;
    }

    public void SetFilmGrainIntensity(float intensity)
    {
        filmGrain.intensity.value = intensity;
    }

    public void StartPaperShredder()
    {
        if (GoBackButton != null) GoBackButton.SetActive(true);
   

        if (GameManager.instance != null /*&& !GameManager.instance.isDebug)*/ && GameManager.instance.personalBannedWord_Day.Count != 0)
        {
            readyToSpawnShredderWordList.AddRange( GameManager.instance.personalBannedWord_Day);
        }

        StartCoroutine(IE_InstantiateWord());
    }

    public void StartPaperShredderWithGivenList(List<string> strings)
    {
        readyToSpawnShredderWordList.AddRange(strings);
        StartCoroutine(IE_InstantiateWord());
    }

    public void InstantiateWord(string s)
    {
        Quaternion q = Quaternion.Euler(Random.Range(0.0f, 10.0f), Random.Range(0.0f, 10.0f), Random.Range(0.0f, 50.0f));
        GameObject w;
        if (Canvas != null)
        {
            w = Instantiate(word, Canvas.transform);
         
        }
        else
        {
            Vector3 pos = new Vector3(this.transform.position.x /*+ Random.Range(-screenWidth / 2, screenWidth / 4)*/, this.transform.position.y, this.transform.position.z);
            w = Instantiate(word, pos, Quaternion.identity);
            
          

        }
        w.GetComponent<ShredderWord>().SetWord(s);
        SpawnedShredderWordList.Add(w.GetComponent<ShredderWord>());

        if (GameManager.instance.GetCurrentAppMode() == GameManager.AppMode.Speed)
        {
            w.transform.localScale = new Vector3(2, 2, 2);
         

        }

        CheckWordFilledAmaountAndSetColor();
    }


    IEnumerator IE_InstantiateWord()
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

    public float CheckWordFilledAmaountAndSetColor()
    {
        Debug.Log("Update filled amount color ");
        float totalArea = 0f;

        ShredderWord[] ShredderWords = FindObjectsOfType<ShredderWord>();
        float fillPrecent = 0;
        if (GameManager.instance.GetCurrentAppMode() == GameManager.AppMode.Story)
        {
            for (int i = 0; i < ShredderWords.Length; i++)
            {
                float area = ShredderWords[i].GetArea();
                totalArea += area;
            }

            fillPrecent = (totalArea / screenArea) * 100;
            


            if (fillPrecent >= maxFilledPrecent)
            {
                ScreenFilled();
            }
        }
        else
        {
            fillPrecent = (ShredderWords.Length/ maxFilledWord) * 100;
            Debug.Log("Fill precent: " + fillPrecent);
            if (ShredderWords.Length > maxFilledWord)
            {
                ScreenFilled();
            }

            if (fillPrecent > 60)
            {
                // Set the "blink" bool parameter to true in the animator
                ciga_animatorController.SetBool("blink", true);
            }
            else
            {
                // Set the "blink" bool parameter to false in the animator
                ciga_animatorController.SetBool("blink", false);
            }
        }

        SetVignetteIntensity(Mathf.Clamp(fillPrecent / 100, 0.2f, 0.9f));
        SetFilmGrainIntensity(fillPrecent / 100);
        if (AnalogGlitch != null)
            AnalogGlitch.scanLineJitter = new ClampedFloatParameter((fillPrecent / 100) / 2.3f, 0.005f, 0.4f);

        for (int i = 0; i < ShredderWords.Length; i++)
        {
            Color color = Color.Lerp(startColor, endColor, fillPrecent / 100);
            ShredderWords[i].SetWordColor(color);

        }

        return fillPrecent;
    }

    public void ClearWordFilledAmaountAndSetColor()
    {

        SetVignetteIntensity(Mathf.Clamp(0 / 100, 0.2f, 0.9f));
        SetFilmGrainIntensity(0 / 100);
        if (AnalogGlitch != null)
            AnalogGlitch.scanLineJitter = new ClampedFloatParameter((0 / 100) / 2.3f, 0.005f, 0.4f);

    }
    void ScreenFilled()
    {
        // Your function to fire when objects have filled up the whole screen goes here
        Debug.Log("Screen filled!");
        GameManager.instance.SpeedRunGameOver();
    }

    public void ChangeGravity()
    {
        Debug.Log("Change Gravity");
        Physics2D.gravity = new Vector3(0f, negativeGravity, 0f);
        foreach (ShredderWord s in SpawnedShredderWordList)
        {
            if(GameManager.instance.GetCurrentAppMode() == GameManager.AppMode.Story)
                s.TurnToWhite();
            else if (GameManager.instance.GetCurrentAppMode() == GameManager.AppMode.Speed)
                s.TurnToWhiteAndDisappear();

            
        }

     
    }

    public void ChangeGravityBack()
    {
        Debug.Log("Change Gravity Back");
        Physics2D.gravity = new Vector3(0f, -negativeGravity, 0f);
        foreach (ShredderWord s in SpawnedShredderWordList)
        {
            Destroy(s.gameObject);
        }

        SpawnedShredderWordList.Clear();
        /*(foreach (ShredderWord s in FindObjectsOfType<ShredderWord>())
        {
            s.TurnToWhite();
        }*/
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//[System.Serializable]
public class UnityAnimationEvent : UnityEvent<string> { };
public class PoemPaperController : MonoBehaviour
{
    public enum PoemPaperMode
    { 
        Read,
        Write,
    }
   
    [HideInInspector] public UnityAnimationEvent OnAnimationStart = new UnityAnimationEvent();
    [HideInInspector] public UnityAnimationEvent OnAnimationComplete = new UnityAnimationEvent();

    [SerializeField] GameObject PassStamp;
    [SerializeField] GameObject DenyStamp;

    public Animator PoemCanvasAnimator;

    public UnityEvent OnPaperExitFinish = new UnityEvent();

    public PoemPaperMode poemPaperMode = PoemPaperMode.Read;
    public PoemGenerator poemGenerator;

    [Header("UI Reference_Write")]
    [HideInInspector]public GameObject PoemParent_Write;
    [HideInInspector] public GameObject PoemPaper_Write;
    [HideInInspector] public PoemPaperController poemPaperController_Write;

    public Poem CurrentPoemOnCanvas;
    public List<PoemLine> poemLinesList = new List<PoemLine>();

    int currentSelectLineIndex = 0;

    public GameObject Paragarphy;

    // Start is called before the first frame update
    void Start()
    {
        if (poemPaperMode == PoemPaperMode.Read)
        {
            PoemCanvasAnimator = this.GetComponent<Animator>();
            //Bind all Animation cips with animation start and end event
            for (int i = 0; i < PoemCanvasAnimator.runtimeAnimatorController.animationClips.Length; i++)
            {
                AnimationClip clip = PoemCanvasAnimator.runtimeAnimatorController.animationClips[i];

                AnimationEvent animationStartEvent = new AnimationEvent();
                animationStartEvent.time = 0;
                animationStartEvent.functionName = "AnimationStartHandler";
                animationStartEvent.stringParameter = clip.name;

                AnimationEvent animationEndEvent = new AnimationEvent();
                animationEndEvent.time = clip.length;
                animationEndEvent.functionName = "AnimationCompleteHandler";
                animationEndEvent.stringParameter = clip.name;

                clip.AddEvent(animationStartEvent);
                clip.AddEvent(animationEndEvent);
            }

            if (OnAnimationStart != null)
            {
                OnAnimationStart.AddListener(SwitchOnAnimationStart);

            }

            if (OnAnimationComplete != null)
                OnAnimationComplete.AddListener(SwitchOnAnimationEnd);

            OnPaperExitFinish.AddListener(OnPoemPaperExitAnimationEnd);

        }
        else if (poemPaperMode == PoemPaperMode.Write)
        {
            FindObjectOfType<PoemGenerator>().AssignWriteModeReference(PoemParent_Write, PoemPaper_Write, this.GetComponent<Animator>(), poemPaperController_Write);
        }
       
        poemGenerator = FindObjectOfType<PoemGenerator>();
        GameManager.instance.OnPoemPass.AddListener(OnPoemPass);
    }

    public void SwitchOnAnimationStart(string name)
    {
        //Excute when Animation Clip Start, name is the name of the clip
        switch (name)
        {
            case "PoemPaperEnter":
                OnPoemPaperEnterAnimationStart();
                break;
            case "PoemPaperExit":
                OnPoemPaperExitAnimationStart();
                break;

        }
        Debug.Log(name);
    }

    public void SwitchOnAnimationEnd(string name)
    {
        //Excute when Animation Clip Complete, name is the name of the clip
        switch (name)
        {
            case "PoemPaperEnter":
                OnPoemPaperEnterAnimationEnd();
                break;
            case "PoemPaperExit":
                OnPaperExitFinish.Invoke();
                break;

        }
        Debug.Log(name);
    }

    public void OnPoemPaperEnterAnimationEnd()
    {
        WorkViewController workViewController = FindObjectOfType<WorkViewController>();
        if (workViewController == null) return;
        workViewController.SetDenyButtonActive(true);
        workViewController.SetPassButtonActive(true);
    }

    public void OnPoemPaperExitAnimationStart()
    {
        WorkViewController workViewController = FindObjectOfType<WorkViewController>();
        if (workViewController == null) return;
        workViewController.SetDenyButtonActive(false);
        workViewController.SetPassButtonActive(false);
    }

    public void OnPoemPaperExitAnimationEnd()
    {
        if (DenyStamp != null) DenyStamp.SetActive(false);
        if (PassStamp != null) PassStamp.SetActive(false);

    }

    public void OnPoemPaperEnterAnimationStart()
    {
        if (DenyStamp != null) DenyStamp.SetActive(false);
        if (PassStamp != null) PassStamp.SetActive(false);
    }


    public void OnPoemPass()
    {

        if (PassStamp != null) PassStamp.SetActive(true);
    }

    public void OnPoemDeny()
    {
        if (DenyStamp != null) DenyStamp.SetActive(true);
    }


    ///////////////////////////
    /////////Animation/////////
    ///////////////////////////

    //Invoke when Animation clip start, name is the name of the clip
    public void AnimationStartHandler(string name)
    {
        //Debug.Log($"{name} animation start.");
        OnAnimationStart?.Invoke(name);
    }

    //Invoke when Animation clip complete, name is the name of the clip
    public void AnimationCompleteHandler(string name)
    {
        //Debug.Log($"{name} animation complete.");
        OnAnimationComplete?.Invoke(name);
    }


    public void TryAddWordToPoem(string s)
    {
        //if (Word_go.GetComponent<Word>() == null) return;
        if (s == "") return;
        poemGenerator.AddWordToPoem(s);
    }

    public void SetCurrentPoem(Poem p)
    {
        CurrentPoemOnCanvas = p;
        currentSelectLineIndex = 0;
        SetSelectLine(currentSelectLineIndex);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {

            UnselectLine(currentSelectLineIndex);
            if (currentSelectLineIndex == poemLinesList.Count - 1)
            {
                currentSelectLineIndex = 0;
            }
            else
            {
               
                currentSelectLineIndex++;
            }

            SetSelectLine(currentSelectLineIndex);
            Debug.Log("next line: " + currentSelectLineIndex);
        }
    }



    public void SetSelectLine(int i)
    {
        //if (i == currentSelectLineIndex) return;
        if (i >= poemLinesList.Count) return;
       
        currentSelectLineIndex = i;
        Vector3 targetScale = new Vector3(1.26f, 1.26f, 1.26f);
        LeanTween.scale(poemLinesList[i].gameObject, targetScale, 0.5f);

        Vector3 targetPosition = new Vector3(Paragarphy.GetComponent<RectTransform>().anchoredPosition.x,
                                             poemLinesList[i].gameObject.GetComponent<RectTransform>().rect.height * i
                                             ,0);

        LeanTween.move(Paragarphy.GetComponent<RectTransform>(), targetPosition, 0.5f);
        // parag
    }

    public void UnselectLine(int i)
    {
        if (i >= poemLinesList.Count) return;
        Vector3 targetScale = new Vector3(0.5f, 0.5f, 0.5f);
        LeanTween.scale(poemLinesList[i].gameObject, targetScale, 0.5f);
    }



}

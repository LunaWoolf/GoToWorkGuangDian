using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
       
        poemGenerator = FindObjectOfType<PoemGenerator>();
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
        workViewController.SetNewsButtonActive(true);
    }

    public void OnPoemPaperExitAnimationStart()
    {
        WorkViewController workViewController = FindObjectOfType<WorkViewController>();
        if (workViewController == null) return;
        workViewController.SetDenyButtonActive(false);
        workViewController.SetPassButtonActive(false);
        workViewController.SetNewsButtonActive(false);
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
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineManager : MonoBehaviour
{
    [System.Serializable]
    public struct timelineShot
    {
        [SerializeField]  public string name;
        [SerializeField]  public TimelineAsset timelineAsset;
    }

    [SerializeField] List<timelineShot> timelineShotList = new List<timelineShot>();
    [SerializeField] public TimelineAsset doorOpenTimeline;
    [SerializeField] public TimelineAsset familyDoorOpenTimeline;

    PlayableDirector playableDirector;

    // Start is called before the first frame update
    void Start()
    {
        playableDirector = GetComponent<PlayableDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayTimeline(TimelineAsset tl)
    {
        Debug.Log("Play timeline : " + tl.name);
        if (tl == null) { Debug.Log("time line is null"); return; }
        playableDirector.playableAsset = tl;
        playableDirector.Play();
    }

    public void PlayTimeLine(int index)
    {

        if (index < 0 || index >= timelineShotList.Count)
        {
            Debug.LogError("Invalid timeline index");
            return;
        }
        Debug.Log("Play timeline : " + index + " " + timelineShotList[index].name);
        TimelineAsset timelineAsset = timelineShotList[index].timelineAsset;
      
        playableDirector.playableAsset = timelineAsset;
        playableDirector.Play();

    }
}

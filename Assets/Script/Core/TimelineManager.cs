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


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        PlayableDirector playableDirector = GetComponent<PlayableDirector>();
        playableDirector.playableAsset = timelineAsset;
        playableDirector.Play();

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Yarn;

public class TVManager : MonoBehaviour
{
    [SerializeField] GameObject TVCanvas;
    [SerializeField] Button CloseTVButton;
    string[] streamPoem;
    [SerializeField] GameObject PoemLine;
    [SerializeField] GameObject PoemParent;

    [SerializeField][TextArea(3,10)] string[] tvOpenning = { "A musician stands in the middle of a vacant stage",
                                              "With a spotlight shining from above",
                                              "They sing:" };

    [SerializeField][TextArea(3,10)] string[] tvNoShow = { "The channel that is supposed to play music is now playing the news.",
                                            "No music has been released recently.",
                                            "Who needs music when there is politics?", "Yeeks." };

    [SerializeField]
    [TextArea(3, 10)]
    string[] Day2TVNews = { "Emerent News:",
                            "We recently get report that a lady was ———————— and",
                            "—————————— around Candy Hill Street",
                            "The police force was there right on time",
                            "Thank for our police no one was harmed"};


    //[SerializeField] TextMeshProUGUI TVText;

    // Start is called before the first frame update

    void Start()
    {
        CloseTVButton.onClick.AddListener(OnCloseTVButtonClosed);
        CloseTVButton.gameObject.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCloseTVButtonClosed()
    {

        FindObjectOfType<DinnerViewController>().OnCloseTV();
    
        TVCanvas.SetActive(false);
       
    }


    public void TryLoadTVProgarm()
    {
        Debug.Log("TryLoadTVProgarm");
        if (streamPoem != null) return;
        TVCanvas.SetActive(true);

        switch (GameManager.instance.GetDay())
        {
            case 0:
                if (GameManager.instance.passPoemCount_day < 1)
                {
                    StartCoroutine(StreamShow(tvNoShow));
                  
                    return;
                }

                if (GameManager.instance.passPoemCount_day > 0)
                {
                    // Get a random poem from today's passed poem
                    int i = Random.Range(0, GameManager.instance.passPoemCount_day - 1);

                    i = PropertyManager.instance.PassedPoem.Count - GameManager.instance.passPoemCount_day + i;
                    i = Mathf.Clamp(i, 0, PropertyManager.instance.PassedPoem.Count);
                    if (i >= 0 && i < PropertyManager.instance.PassedPoem.Count)
                    {
                        streamPoem = PropertyManager.instance.PassedPoem[i];
                        StartCoroutine(StreamPoem());

                        return;
                    }

                    Debug.Log("ReadyToStream");

                }
                return;
                break;

            case 1:

                if (GameManager.instance.passPoemCount_day > 0)
                {

                    int i = Random.Range(0, GameManager.instance.passPoemCount_day - 1);

                    i = PropertyManager.instance.PassedPoem.Count - GameManager.instance.passPoemCount_day + i;
                    i = Mathf.Clamp(i, 0, PropertyManager.instance.PassedPoem.Count);
                    if (i >= 0 && i < PropertyManager.instance.PassedPoem.Count)
                    {
                        streamPoem = PropertyManager.instance.PassedPoem[i];
                        StartCoroutine(StreamPoem());

                        return;
                    }

                    Debug.Log("ReadyToStream");

                }
                else
                {
                    StartCoroutine(StreamShow(Day2TVNews));
                }
                return;
                //Load Day 2 TV Progarm
                break;

            default:
                StartCoroutine(StreamShow(tvNoShow));
                return;
                break;


        }
        CloseTVButton.gameObject.SetActive(true);
    }

    IEnumerator StreamShow(string[] show)
    {
        ClearTVscreen();
        foreach (string line in show)
        {

            PoemLine pline = Instantiate(PoemLine, PoemParent.transform).GetComponent<PoemLine>();
            pline.SetLine(line);
 
            yield return new WaitForSeconds(1f);
           
        }
        CloseTVButton.gameObject.SetActive(true);
    }




    IEnumerator StreamPoem()
    {

        foreach (string line in tvOpenning)
        {

            PoemLine pline = Instantiate(PoemLine, PoemParent.transform).GetComponent<PoemLine>();
            pline.SetLine(line);
            foreach (Word w in pline.GetComponentsInChildren<Word>())
            {
                if (w._UnProcessText[0] == '?')
                {
                    w.SetText("————————");

                }
                w.isCircledable = false;

            }
            yield return new WaitForSeconds(1f);
        }

        foreach (string line in streamPoem)
        {

            PoemLine pline = Instantiate(PoemLine, PoemParent.transform).GetComponent<PoemLine>();
            pline.SetLine(line);
            Word[] words = pline.GetComponentsInChildren<Word>();
            for (int i = 0; i < words.Length; i++)
            {
                Word w = words[i];
                
                if (w._UnProcessText.Length > 0 && w._UnProcessText[0] == '?')
                {
                    w.SetText("————————");
                }
                w.isCircledable = false;

                if (GameManager.instance.GetDay() == 1 && i > 2) // for day 2
                    break;
            }
            yield return new WaitForSeconds(1f);

           
        }

        if (GameManager.instance.GetDay() == 1)
        {
            ClearTVscreen();
            StartCoroutine(StreamShow(Day2TVNews));
        }

        CloseTVButton.gameObject.SetActive(true);

    }

    public void ClearTVscreen()
    {

        int childCount = PoemParent.transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            Transform child = PoemParent.transform.GetChild(i);
            Destroy(child.gameObject);
        }
       
    }
}

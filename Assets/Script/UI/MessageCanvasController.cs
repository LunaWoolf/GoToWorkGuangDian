using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessageCanvasController : MonoBehaviour
{
    [SerializeField] GameObject MessageBlock;
    HeadshotManager headshotManager;
    public Camera cam;

    public bool isOnLeftSide = true;
    
    void Start()
    {
        headshotManager = GetComponent<HeadshotManager>();
        GameManager.instance.onStartWork.AddListener(StartAutoGenerateMessage);
    }

    
    void Update()
    {
        if(GameManager.instance.isDebug && Input.GetKeyDown(KeyCode.M))
            GenerateNewsMessageBlock();
    }

    public void StartAutoGenerateMessage()
    {
        StartCoroutine(GenerateNewsMessageBlock_Auto());
    }
    public void GenerateNewsMessageBlock()
    {
        MessageBlockController m = Instantiate(MessageBlock, this.transform).
                                   GetComponent<MessageBlockController>().Init(MessageBlockController.MessageType.News, NewsManager.instance.GeneratreNews(), " ");
        
        float xPos = 0;
        float yPos = 0;
        if (isOnLeftSide)
        {
            xPos = Random.Range(-Screen.width * 5, -Screen.width * 3);
            yPos = Random.Range(-Screen.height * 4, Screen.height * 4);
        }
        else
        {
            xPos = Random.Range(Screen.width * 3, Screen.width * 6);
            yPos = Random.Range(-Screen.height * 4, Screen.height * 4);
        }
     

        Vector3 spawnPosition = new Vector3(xPos, yPos, 0f);
        //Debug.Log("x  " + xPos);
        //Debug.Log("y  " + yPos);
        m.gameObject.GetComponent<RectTransform>().position = cam.ScreenToViewportPoint(spawnPosition);

        var h = headshotManager.GetRandomHeadShot();
        m.ToogleAndSetHeadShot(true,h.head_sprite,
                                    h.left_eye_position,
                                    h.righ_eye_position);

        isOnLeftSide = !isOnLeftSide;
    }


    IEnumerator GenerateNewsMessageBlock_Auto()
    {
        GenerateNewsMessageBlock();
        yield return new WaitForSeconds(2f);
        if(GameManager.instance.GetCurrentGameMode() == GameManager.GameMode.Work)
            StartCoroutine(GenerateNewsMessageBlock_Auto());
    }


    public void GenerateReviewMessageBlock()
    {
         int digit = Random.Range(1, 6);
         int Plays = Random.Range(0, 1000000) / (10 ^ digit);
         int Sales = Random.Range(0, Plays)/ (10 ^ digit);
         int Likes = Random.Range(0, Plays)/ (10 ^ digit);
         //int Feedback = Random.Range(0, 5);

        string ReviewText = "Plays: " + Plays + "\nSales: " + Sales + "\nLikes: " + Likes;

        MessageBlockController m = Instantiate(MessageBlock, this.transform).
                                    GetComponent<MessageBlockController>().Init(MessageBlockController.MessageType.Review, null, ReviewText);

        float xPos = Random.Range(-Screen.width * 4, 0);
        float yPos = Random.Range(0, Screen.height * 4);

        Vector3 spawnPosition = new Vector3(xPos, yPos, 0f);
        m.GetComponent<RectTransform>().position = cam.ScreenToViewportPoint(spawnPosition);
    }
}

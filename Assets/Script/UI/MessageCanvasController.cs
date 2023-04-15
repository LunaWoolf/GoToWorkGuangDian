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

    
    void Start()
    {
        headshotManager = GetComponent<HeadshotManager>();
    }

    
    void Update()
    {
        if(GameManager.instance.isDebug && Input.GetKeyDown(KeyCode.M))
            GenerateNewsMessageBlock();
    }


    public void GenerateNewsMessageBlock()
    {
        MessageBlockController m = Instantiate(MessageBlock, this.transform).
                                   GetComponent<MessageBlockController>().Init(MessageBlockController.MessageType.News, NewsManager.instance.GeneratreNews(), " ");
        float xPos = Random.Range(-Screen.width * 4, Screen.height * 15);
        float yPos = Random.Range(-Screen.height * 4, Screen.height * 4);

        Vector3 spawnPosition = new Vector3(xPos, yPos, 0f);
        //Debug.Log("x  " + xPos);
        //Debug.Log("y  " + yPos);
        m.GetComponent<RectTransform>().position = cam.ScreenToViewportPoint(spawnPosition);

        var h = headshotManager.GetRandomHeadShot();
        m.ToogleAndSetHeadShot(true,h.head_sprite,
                                    h.left_eye_position,
                                    h.righ_eye_position);

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

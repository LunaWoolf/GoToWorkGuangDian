using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class MessageCanvasController : MonoBehaviour
{
    [SerializeField] GameObject MessageBlock;
    HeadshotManager headshotManager;
    public Camera cam;

    public bool isOnLeftSide = true;
    
    void Start()
    {
        headshotManager = GetComponent<HeadshotManager>();
        //GameManager.instance.onStartWork.AddListener(StartAutoGenerateMessage);
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

        float canvasWidth = this.GetComponent<RectTransform>().rect.width;
        float canvasHeight = this.GetComponent<RectTransform>().rect.height;

        float xOffset = 0;
        float yOffset = 0;
        if (isOnLeftSide)
        {
            xOffset = Random.Range(-3f, -3.5f);
            yOffset = Random.Range(-3, 6);
        }
        else
        {
            xOffset = Random.Range(7.5f, 9);
            yOffset = Random.Range(-3, 4);
        }

        xOffset = Random.Range(-100f,100f);
        yOffset = Random.Range(-50, 50);

        m.gameObject.GetComponent<RectTransform>().position += new Vector3(xOffset, yOffset, 0);


        /*float xPos = 0;
        float yPos = 0;
        if (isOnLeftSide)
        {
            xPos = Random.Range(0, 0);
            yPos = Random.Range(-0, 0);
        }
        else
        {
            xPos = Random.Range(-Screen.width /2, Screen.width /2);
            yPos = Random.Range(-Screen.height /2, Screen.height /2);
        }*/

        //Vector3 spawnPosition = new Vector3(xPos, yPos, 0);

        //xPos = canvasWidth / 2f + screenSpacePosition.x;
        //yPos = canvasHeight / 2f  + screenSpacePosition.y;

        //Vector2 spawnPosition = new Vector2(xPos, yPos);

        //m.gameObject.GetComponent<RectTransform>().position = cam.ScreenToViewportPoint(spawnPosition);
        //m.gameObject.GetComponent<RectTransform>().anchoredPosition = this..GetComponent<RectTransform>().sizeDelta;

        //Debug.Log("x  " + xPos);
        //Debug.Log("y  " + yPos);
        //Vector3 pos;
        //RectTransformUtility.ScreenPointToWorldPointInRectangle(m.gameObject.GetComponent<RectTransform>(), spawnPosition, cam, out pos);
        //m.gameObject.GetComponent<RectTransform>().position = pos;
        //S


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

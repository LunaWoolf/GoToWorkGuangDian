using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using Yarn;

public class MessageCanvasController_Speed : MonoSingleton<MessageCanvasController_Speed>
{
    [SerializeField] GameObject MessageBlock_Speed_left;
    [SerializeField] GameObject MessageBlock_Speed_right;
    HeadshotManager headshotManager;
    public Camera cam;

    public float xOffset_min_left;
    public float xOffset_max_left;
    public float xOffset_min_right;
    public float xOffset_max_right;
    public float yOffset_min;
    public float yOffset_max;

    public bool isOnLeftSide = true;

    public bool isStartGenerateRamdomMessage = true;

    public int OnePoemSpeedmessageCount = 3;


    [SerializeField] string[] Politic_reply;
    [SerializeField] string[] Violent_reply;
    [SerializeField] string[] Sexual_reply;
    [SerializeField] string[] Unreal_reply;
    [SerializeField] string[] Negative_reply;
    [SerializeField] string[] Good_reply;

    [SerializeField] List<string> replies = new List<string>();

    void Start()
    {
        headshotManager = GetComponent<HeadshotManager>();
        if(isStartGenerateRamdomMessage)
            StartAutoGenerateMessage();
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            OnPoemPass();
    }

    public void OnPoemPass()
    {
        SetReplyText();
        if (replies.Count > 1)
        {
            GenerateSpeedMessageBlock(0, replies[0]);
            GenerateSpeedMessageBlock(1, replies[1]);
        }
        
        /*for (int i = 0; i < OnePoemSpeedmessageCount; i++)
        {
            int leftOrright = Random.Range(0, 2);
            GenerateSpeedMessageBlock(leftOrright, replies[i]);
        }*/
        //GenerateSpeedMessageBlock();
    }

    public void StartAutoGenerateMessage()
    {
        StartCoroutine(GenerateNewsMessageBlock_Auto());
    }

    public void SetReplyText()
    {
        List<Word.WordQuality> qualities = FindObjectOfType<WorkViewController>().CurrentPoemOnCanvas.CheckPoemQuality();
        int rand2 = 0;
        if (qualities.Count == 0)
        {
            for (int i = 0; i < OnePoemSpeedmessageCount; i++)
            {
                rand2 = Random.Range(0, Good_reply.Length);
                replies.Add(Good_reply[rand2]);
                return;
            }
        } 

        for(int i = 0; i < OnePoemSpeedmessageCount; i++)
        {
            int rand = Random.Range(0, qualities.Count);
           
            switch (qualities[rand])
            {
                case Word.WordQuality.Politic:
                    rand2 = Random.Range(0, Politic_reply.Length);
                    replies.Add(Politic_reply[rand2]);
                    break;
                case Word.WordQuality.Violent:
                    rand2 = Random.Range(0, Violent_reply.Length);
                    replies.Add(Violent_reply[rand2]);
                    break;
                case Word.WordQuality.Sexual:
                    rand2 = Random.Range(0, Sexual_reply.Length);
                    replies.Add(Sexual_reply[rand2]);
                    break;
                case Word.WordQuality.Unreal:
                    rand2 = Random.Range(0, Unreal_reply.Length);
                    replies.Add(Unreal_reply[rand2]);
                    break;
                case Word.WordQuality.Negative:
                    rand2 = Random.Range(0, Negative_reply.Length);
                    replies.Add(Negative_reply[rand2]);
                    break;
                case Word.WordQuality.Good:
                    rand2 = Random.Range(0, Good_reply.Length);
                    replies.Add(Good_reply[rand2]);
                    break;
            }

        }
     

    }

    public void GenerateSpeedMessageBlock(int leftOrright, string replyText)
    {
        string ReplyText = replyText;

       
        MessageBlockController_Speed m;
        if (leftOrright == 0)
            m = Instantiate(MessageBlock_Speed_left, this.transform).
                                    GetComponent<MessageBlockController_Speed>().Init(MessageBlockController_Speed.MessageType.Reply, null, ReplyText);
        else
            m = Instantiate(MessageBlock_Speed_right, this.transform).
                                  GetComponent<MessageBlockController_Speed>().Init(MessageBlockController_Speed.MessageType.Reply, null, ReplyText);

        float xOffset = 0;
        if (leftOrright == 0)
        {
            xOffset = Random.Range(xOffset_min_left, xOffset_max_left);
        }

        else
        {
            xOffset = Random.Range(xOffset_min_right, xOffset_max_right);
        }


           
        float yOffset = Random.Range(yOffset_min, yOffset_max);

        m.gameObject.GetComponent<RectTransform>().position += new Vector3(xOffset, yOffset, 0);

        var h = headshotManager.GetRandomHeadShot();
        m.ToogleAndSetHeadShot(true, h.head_sprite,
                                    h.left_eye_position,
                                    h.righ_eye_position);

    }



    IEnumerator GenerateNewsMessageBlock_Auto()
    {
       // GenerateNewsMessageBlock();
        yield return new WaitForSeconds(2f);
        if(GameManager.instance.GetCurrentGameMode() == GameManager.GameMode.Bus)
            StartCoroutine(GenerateNewsMessageBlock_Auto());
    }


   
}

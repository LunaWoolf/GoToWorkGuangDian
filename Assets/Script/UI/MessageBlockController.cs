using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class MessageBlockController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI NewsTitle;
    [SerializeField] TextMeshProUGUI NewsBody;
    [SerializeField] Image BackgroundImage;

    [Header("Show Head Shot")]
    [SerializeField] GameObject headShot;
    [SerializeField] public bool isShowHeadShot;
    [SerializeField] Image headshotImage;
    [SerializeField] GameObject LeftEye;
    [SerializeField] GameObject RightEye;

    float DisappearTime = 3f;
 
    [SerializeField] float DisappearTime_news = 3f;
    [SerializeField] float DisappearTime_reply = 3f;

    [SerializeField] MessageType messageType = MessageType.News;

    public void SetMessageType(MessageType t)
    {

        messageType = t;
    }


    public MessageBlockController Init(MessageType type, News news, string text)
    {
        SetMessageType(type);
        switch (type)
        {
            case MessageType.News:
                SetNewsMessage(news);
                DisappearTime = DisappearTime_news;
                break;
            case MessageType.Review:
                headShot.SetActive(false);
                isShowHeadShot = false;
                SetReviewMessage(text);
                DisappearTime = DisappearTime_news;
                break;
            case MessageType.Reply:
                headshotImage.color = new Color(0, 0, 0, 1);
                SetReplyMessage(text);
                DisappearTime = DisappearTime_reply;
                break;
        }
        return this;
    }

    public enum MessageType
    {
        News,
        Review,
        Reply,
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SelfDestroy());
        SetMessageType(messageType);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(this.transform.position.x + Random.Range(-0.02f, 0.02f), this.transform.position.y + Random.Range(-0.02f, 0.02f), this.transform.position.z);
        BackgroundImage.color += new Color(Random.Range(-0.01f, 0.01f), Random.Range(-0.01f, 0.01f), Random.Range(-0.01f, 0.01f));
    }

    public void SetNewsMessage(News n)
    {
        NewsTitle.text = n.title;
        NewsBody.text = n.content;

    }

    public void SetReviewMessage(string text)
    {
        NewsTitle.text = text;
    }

    public void SetReplyMessage(string text)
    {
        NewsTitle.text = text;
       
    }

    IEnumerator SelfDestroy()
    {
        yield return new WaitForSeconds(DisappearTime);
        Destroy(this.gameObject);
    }

    public void ToogleAndSetHeadShot(bool isOn, Sprite image,Vector3 LeftEyePos, Vector3 RightEyePos)
    {
        isShowHeadShot = isOn;
        headShot.SetActive(isOn);
        if (isOn)
        {
            headshotImage.sprite = image;
            LeftEye.GetComponent<RectTransform>().localPosition = LeftEyePos;
            RightEye.GetComponent<RectTransform>().localPosition = RightEyePos;
        }
    }

}

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

    [SerializeField] float DisappearTime = 5f;

    [SerializeField] MessageType messageType = MessageType.News;

    public void SetMessageType(MessageType t)
    {

        messageType = t;
    
    }


    public MessageBlockController Init(MessageType type, News news, string review)
    {
        SetMessageType(type);
        switch (type)
        {
            case MessageType.News:
                SetNewsMessage(news);
                break;
            case MessageType.Review:
                headShot.SetActive(false);
                isShowHeadShot = false;
                SetReviewMessage(review);
                break;
        }
        return this;
    }

    public enum MessageType
    {
        News,
        Review,
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
        this.transform.position = new Vector3(this.transform.position.x + Random.Range(-0.01f, 0.01f), this.transform.position.y + Random.Range(-0.01f, 0.01f), 0);
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
        //NewsBody.text = n.content;
    }


    IEnumerator SelfDestroy()
    {
        yield return new WaitForSeconds(DisappearTime);
        Destroy(this.gameObject);
    }

    public void ToogleHeadShot(bool isOn)
    {
        isShowHeadShot = isOn;
        headShot.SetActive(isOn);
    }
}

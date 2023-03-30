using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessageCanvasController : MonoBehaviour
{
    [SerializeField] GameObject MessageBlock;
    public Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //GenerateMessageBlock();
    }

    public void UpdateMessage(News n)
    {
        //NewsTitle.text = n.title;
        //NewsBody.text = n.content;
        //NewsImage = n.Art;
    
    }

    public void GenerateMessageBlock()
    {
        GameObject m = Instantiate(MessageBlock, this.transform);
        float xPos = Random.Range(0, Screen.width);
        float yPos = Random.Range(0, Screen.height);
        Vector3 spawnPosition = new Vector3(xPos, yPos, 0f);
        m.transform.position = cam.ScreenToViewportPoint(spawnPosition);

        m.GetComponent<MessageBlockController>().SetMessage(NewsManager.instance.GeneratreNews());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessageCanvasController : MonoBehaviour
{
    [SerializeField] GameObject MessageBlock;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

    }
}

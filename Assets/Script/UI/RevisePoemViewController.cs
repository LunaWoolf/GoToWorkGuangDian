using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static PoemFile;

public class RevisePoemViewController : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI tm;
    public RawImage replyImage;
    public float typingSpeed = 0.05f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator TypeText()
    {
        // Get the total number of characters in the text
        int totalCharacters = tm.text.Length;

        // Start with no visible characters
        tm.maxVisibleCharacters = 0;

        // Reveal characters one by one
        for (int i = 0; i <= totalCharacters; i++)
        {
            tm.maxVisibleCharacters = i;  // Update the number of visible characters
            yield return new WaitForSeconds(typingSpeed);  // Wait before showing the next character
        }
      
    }


    public void SetReplyImage(Texture SpeicalImage)
    {
        replyImage.texture = SpeicalImage;


    }

    public void SetReplyText(string SpeicalReply)
    {
        tm.maxVisibleCharacters = 0;
        tm.text = SpeicalReply;
        StartCoroutine(TypeText());
    }
}

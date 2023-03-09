using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WriteCanvasContoller : MonoBehaviour
{
    [SerializeField] Button FinishButton;
    [SerializeField] Button NewLineButton;
    // Start is called before the first frame update
    void Start()
    {
        AddBasicWordToWordBand();
        FinishButton.onClick.AddListener(OnFinishButtonClicked);
        NewLineButton.onClick.AddListener(OnNewLineButtonClicked);
    }

    void OnFinishButtonClicked()
    {
        //Save Poem
        PropertyManager.instance.bHasWritePoem = true;
        FindObjectOfType<PoemGenerator>().MoveWritePoemToReadPoem();
        GameManager.instance.GoToNextWorkDay();
        //temp fix
        Destroy(this.gameObject);
    }

    void OnNewLineButtonClicked()
    {
        if(FindObjectOfType<PoemGenerator>() != null)
            FindObjectOfType<PoemGenerator>().AddNewLineToPoem();
  
    }


    void AddBasicWordToWordBand()
    {
        GameManager.instance.AddWordToWordList("I");
        GameManager.instance.AddWordToWordList("You");
        GameManager.instance.AddWordToWordList("want");
        GameManager.instance.AddWordToWordList("to");
        GameManager.instance.AddWordToWordList("can");
        GameManager.instance.AddWordToWordList("can't");
        GameManager.instance.AddWordToWordList("but");
        GameManager.instance.AddWordToWordList("will");
        GameManager.instance.AddWordToWordList("how");
        GameManager.instance.AddWordToWordList("no");
        GameManager.instance.AddWordToWordList("let");
        //GameManager.instance.AddWordToWordList("let");
    }
}

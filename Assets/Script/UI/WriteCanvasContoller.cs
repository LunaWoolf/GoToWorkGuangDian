using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WriteCanvasContoller : MonoBehaviour
{
    [SerializeField] Button FinishButton;
    [SerializeField] Button NewLineButton;
    [SerializeField] Button DeleteLineButton;
    PoemGenerator poemGenerator;
    // Start is called before the first frame update
    void Start()
    {
        poemGenerator = FindObjectOfType<PoemGenerator>();
        AddBasicWordToWordBand();
        FinishButton.onClick.AddListener(OnFinishButtonClicked);
        NewLineButton.onClick.AddListener(OnNewLineButtonClicked);
        DeleteLineButton.onClick.AddListener(OnDeleteButtonClicked);

        // poemGeneratorGenerateEmptyLine()
    }

    void OnFinishButtonClicked()
    {
        //Save Poem
        PropertyManager.instance.bHasWritePoem = true;
        poemGenerator.MoveWritePoemToReadPoem();
        //temp fix
        //Destroy(this.gameObject);

        //Unload Write Scene
        ScenesManager.instance.UnloadScene("WriteScene");


    }

    void OnDeleteButtonClicked()
    {
        if (poemGenerator != null)
            poemGenerator.DeleteLastWord();

    }

    void OnNewLineButtonClicked()
    {
        if(poemGenerator != null)
            poemGenerator.AddNewLineToPoem();
  
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

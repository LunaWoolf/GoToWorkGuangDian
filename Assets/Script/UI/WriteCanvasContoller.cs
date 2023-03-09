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
}

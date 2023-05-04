using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Yarn;

public class TVManager : MonoBehaviour
{
    [SerializeField] GameObject TVCanvas;
    [SerializeField] Button CloseTVButton;
    string[] streamPoem;
    [SerializeField] GameObject PoemLine;
    [SerializeField] GameObject PoemParent;

    [SerializeField] string[] tvOpenning = { "A musician stands in the middle of a vacant stage", "With a spotlight shining from above", "They sing:" };

    //[SerializeField] TextMeshProUGUI TVText;

    // Start is called before the first frame update
    void Start()
    {
        CloseTVButton.onClick.AddListener(OnCloseTVButtonClosed);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCloseTVButtonClosed()
    {
        TVCanvas.SetActive(false);
    }


    public void TryLoadTVProgarm()
    {
        Debug.Log("TryLoadTVProgarm");
        if (streamPoem != null) return;
        TVCanvas.SetActive(true);
        if (GameManager.instance.passPoemCount_day < 1)
        {
            LocalDialogueManager.instance.LoadDialogue("TV_NoShow");
            return;
        }

        if (GameManager.instance.passPoemCount_day > 0)
        {
            // Get a random poem from today's passed poem
            int i = Random.Range(0, GameManager.instance.passPoemCount_day - 1);

            i = PropertyManager.instance.PassedPoem.Count - GameManager.instance.passPoemCount_day + i;
            i = Mathf.Clamp(i, 0, PropertyManager.instance.PassedPoem.Count);
            if (i >= 0 && i < PropertyManager.instance.PassedPoem.Count)
            {
                streamPoem = PropertyManager.instance.PassedPoem[i];
                StartCoroutine(StreamPoem());
         
                return;
            }

            Debug.Log("ReadyToStream");

        }
    }

    IEnumerator StreamPoem()
    {

        foreach (string line in tvOpenning)
        {

            PoemLine pline = Instantiate(PoemLine, PoemParent.transform).GetComponent<PoemLine>();
            pline.SetLine(line);
            foreach (Word w in pline.GetComponentsInChildren<Word>())
            {
                if (w._UnProcessText[0] == '?')
                {
                    w.SetText("————————");

                }
                w.isCircledable = false;

            }
            yield return new WaitForSeconds(1f);
        }

        foreach (string line in streamPoem)
        {

            PoemLine pline = Instantiate(PoemLine, PoemParent.transform).GetComponent<PoemLine>();
            pline.SetLine(line);
            foreach (Word w in pline.GetComponentsInChildren<Word>())
            {
                if (w._UnProcessText[0] == '?')
                {
                    w.SetText("————————");
                    
                }
                w.isCircledable = false;

            }
            yield return new WaitForSeconds(1f);
        }

     
    }
}

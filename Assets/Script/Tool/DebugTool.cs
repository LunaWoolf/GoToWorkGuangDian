using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using TMPro;

public class DebugTool : MonoSingleton<DebugTool>
{
    private int tapCount;
    private bool isCounting;

    [SerializeField] GameObject DebugCanvas;
    [SerializeField] TextMeshProUGUI DebugCanvas_Text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private IEnumerator StartTapCount()
    {
        yield return new WaitForSeconds(3f);

    
        tapCount = 0;
        isCounting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.GetCurrentAppMode() == GameManager.AppMode.Expo)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                DebugCanvas.SetActive(!DebugCanvas.activeSelf);
            }
            return;
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            if (!isCounting)
            {
                StartCoroutine(StartTapCount());
                isCounting = true;
            }
            tapCount++;

            if (tapCount >= 5)
            {
                tapCount = 0;
                GameManager.instance.isDebug = !GameManager.instance.isDebug;
            }

            GameManager.instance.StartChapter();
        }

        if (GameManager.instance.isDebug)
        {

          

            if (Input.GetKeyDown(KeyCode.W))
            {
                GameManager.instance.StartWork();
            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                if (FindObjectOfType<PaperShredderManager>())
                    FindObjectOfType<PaperShredderManager>().ChangeGravity();
            }


           
            if (Input.GetKeyDown(KeyCode.P))
            {
                GameManager.instance.StartPaperShredder();
            }

            if (Input.GetKeyDown(KeyCode.N))
            {
                NewsManager.instance.RefreshCureentValidNews();
                NewsManager.instance.GeneratreNews();
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                //Bus
                ViewManager.instance.UnloadAllView();
                GameManager.instance.GoToBus();
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                //Dinner
                ViewManager.instance.UnloadAllView();
                GameManager.instance.GoToDinner();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                //Say
                ViewManager.instance.UnloadAllView();
                GameManager.instance.GoToSaySomething();
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                //Say
                //ViewManager.instance.UnloadAllView();
                //GameManager.instance.GoToLake();
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                GameManager.instance.SetDay(0);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                GameManager.instance.SetDay(1);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                GameManager.instance.SetDay(2);
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                GameManager.instance.SetDay(3);
            }

            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                GameManager.instance.SetDay(4);
            }

            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                GameManager.instance.SetDay(5);
            }
        }

    }

    public void SetDebugCanvasText(string text)
    {
        DebugCanvas_Text.text = text;
    }
}

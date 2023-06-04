using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class DebugTool : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            GameManager.instance.isDebug = !GameManager.instance.isDebug;
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
}

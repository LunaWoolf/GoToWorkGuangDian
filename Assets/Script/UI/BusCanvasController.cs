using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BusCanvasController : MonoBehaviour
{
    [SerializeField] Button GoHomeButton;

    void Start()
    {
        GoHomeButton.onClick.AddListener(GameManager.instance.GoToDinner);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

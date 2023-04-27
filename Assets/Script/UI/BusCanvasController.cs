using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BusCanvasController : MonoBehaviour
{
    [SerializeField] Button GoHomeButton;
    [SerializeField] Slider BusSlider;
    [SerializeField] float busTime;

    float timePassed = 0;

    void Start()
    {
        GoHomeButton.onClick.AddListener(GameManager.instance.GoToDinner);
        GoHomeButton.gameObject.SetActive(false);
        if (GameManager.instance.isDebug)
            GoHomeButton.gameObject.SetActive(true);
    }

 

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        BusSlider.value = timePassed / busTime;
        if (timePassed > busTime && !GoHomeButton.gameObject.activeSelf)
            GoHomeButton.gameObject.SetActive(true);
    }
}

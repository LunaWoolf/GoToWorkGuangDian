using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewsCanvasController : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI NewsTitle;
    [SerializeField] TextMeshProUGUI NewsBody;
    [SerializeField] Image NewsImage;
    [SerializeField] Button CloseButton;

    void Start()
    {
        CloseButton.onClick.AddListener(OnCloseButtonClicked);
    }

    // Update is called once per frame
    public void UpdateNews(News n)
    {
        NewsTitle.text = n.title;
        NewsBody.text = n.content;
        NewsImage = n.Art;

    }

    void ClearNewsView()
    {
        NewsTitle.text = "";
        NewsBody.text = "";
        NewsImage.sprite = null;

    }

    void OnCloseButtonClicked()
    {
        ClearNewsView();
        GameManager.instance.GoBackToWork();
    }
}

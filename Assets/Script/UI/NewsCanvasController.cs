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
    [SerializeField] Button NextButton;
    [SerializeField] int currentSessionViewCount = 0;

    void Start()
    {
        if(CloseButton != null) CloseButton.onClick.AddListener(OnCloseButtonClicked);
        if (NextButton != null) NextButton.onClick.AddListener(OnNextButtonClicked);

    }

    private void OnEnable()
    {
        currentSessionViewCount = 0;
        NextButton.gameObject.SetActive(true);
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
        if(NewsImage != null) NewsImage.sprite = null;

    }

    void OnCloseButtonClicked()
    {
        ClearNewsView();
        if (GameManager.instance.GetCurrentGameMode() == GameManager.GameMode.Work)
        {
            GameManager.instance.AdjustAndCheckWorkActionCountOfDay(1);
            GameManager.instance.GoBackToWork();
        }
        if (GameManager.instance.GetCurrentGameMode() == GameManager.GameMode.SaySomething)
        {
            LocalDialogueManager.instance.LoadDialogue("GoToSleepAfterReadNews");
        }

    }

    void OnNextButtonClicked()
    {
        NewsManager.instance.RefreshCureentValidNews();
        NewsManager.instance.GeneratreNews();
        currentSessionViewCount += 1;

        if (currentSessionViewCount >= 2)
        {
            NextButton.gameObject.SetActive(false);
        }
    }
}

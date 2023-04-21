using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class NewsManager : MonoSingleton<NewsManager>
{
    [SerializeField] public List<News> NewsList;

    List<News> currentValidNews = new List<News>();

    [SerializeField] public NewsCanvasController newsCanvasController;

    void Start()
    {
        RefreshCureentValidNews();
        if(newsCanvasController == null)
            newsCanvasController = FindObjectOfType<NewsCanvasController>();
    }


    void Awake()
    {
        var objs = FindObjectsOfType<NewsManager>();

        if (objs.Length > 1)
        {
            foreach (var v in objs)
            {
                if (v.gameObject != this.gameObject)
                    Destroy(v.gameObject);
            }
        }

        DontDestroyOnLoad(this.gameObject);

    }
    void Update()
    {
        
    }

    public void RefreshCureentValidNews()
    {
        currentValidNews.Clear();
        foreach (News n in NewsList)
        {
            if (CheckNewsRestriction(n))
                currentValidNews.Add(n);
        }

    }

    public News GeneratreNews()
    {
        int rand = Random.Range(0, currentValidNews.Count);
        News currentNews = currentValidNews[rand];
        News AfterProcessNews = new News();

        AfterProcessNews.title = ProcessNewsInfo(currentNews.title);
        AfterProcessNews.content = ProcessNewsInfo(currentNews.content);
        AfterProcessNews.Art = currentNews.Art;

        Debug.Log("title: " + AfterProcessNews.title);
        Debug.Log("content: " + AfterProcessNews.content);
        

        if (newsCanvasController == null)
            newsCanvasController = FindObjectOfType<NewsCanvasController>();
        if (newsCanvasController != null)
            newsCanvasController.UpdateNews(AfterProcessNews);

        return AfterProcessNews;
    }

    string ProcessNewsInfo(string line)
    {
        string result = " ";
        string[] list = Regex.Split(line, " ");
        if (list.Length > 0)
        {
            foreach (string s in list)
            {
                string r = s;

                if (s.Contains("<banned_word>"))
                {
                    r = s.Replace("<banned_word>", GameManager.instance.GetRandomBannedWord());
                }
                if (s.Contains("<pass_song>"))
                {
                    r = "";
                    string[] poem = PropertyManager.instance.GetRandomPassedPoem();
                    if (poem != null)
                    {
                        foreach (string p_line in poem)
                        {
                            r += p_line + "\n";
                        }

                    }
                   
                }
              

                result += r + " ";

            }
        }
        return result;

    }

    bool CheckNewsRestriction(News n)
    {
        foreach (News.restrication r in n.restricationList)
        {
            switch (r.property)
            {
                case "pass_song_count":
                    if (PropertyManager.instance.PassedPoem.Count < r.min || PropertyManager.instance.PassedPoem.Count > r.max)
                        return false;
                    break;

                case "banned_word_count":
                    if (GameManager.instance.personalBannedWordMap.Count-1 < r.min || GameManager.instance.personalBannedWordMap.Count-1 > r.max) // cause the first item in map is "Default"
                        return false;
                break;

            }
        }
        return true;
    }

  
}

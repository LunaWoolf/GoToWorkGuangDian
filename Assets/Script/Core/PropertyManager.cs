using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertyManager : MonoSingleton<PropertyManager>
{
    [Header("Player Info")]
    [SerializeField]
    public string player_name = "default";
    
    [SerializeField] public int society_Artistic_Freedom;

    [SerializeField] public int personal_awakening;

    public List<string[]> PassedPoem = new List<string[]>();
    public List<string[]> DeniedPoem = new List<string[]>();

    [Header("Work Change")]
    public bool hasCATgpt = false;
    //public int rebelliousCount = 0;
    public int currentPoemBannedWord = 0;

    [Header("Saved Process")]
    public bool hasShownWorkTutorial = false;
    public bool hasShownNewsTutorial = false;

    [Header("After Work")]
    public bool bCanListenToMusic = false;
    public bool bHasListenToMusic = false;
    public bool bCanReadNews = false;

    [Header("Unlock Writing")]
    public bool  bReflection= false;
    public bool  bAesthetic = false;
    public bool  bRebellious = false;
    public bool  bCanWrite = false;

    [Header("Endign")]
    public bool bHasWritePoem = false;

    [Header("SaySomething")]
    public List<string> writeLines = new List<string>();
    public int cigaretteCount = 0;

    [Header("General")]
    public int money = 20;

    public int GetMoney() { return money; }

    public void UpdateMoney(int changeValue)
    {
        money += changeValue;
     
        ViewManager.instance.SetMoneyText(PropertyManager.instance.money);
        if (money < 0)
            GameManager.instance.SpeedRunGameOver();
    }



    void Awake()
    {
        var objs = FindObjectsOfType<PropertyManager>();

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

    public void Start()
    {
        UpdateMoney(0);

    }

    public string[] GetRandomPassedPoem()
    {
        if (PassedPoem.Count <= 0)
        {
            Debug.Log("There is no passed poem in Get Random Passed Poem");
            return null;

        }
        string[] poem;
        int i = UnityEngine.Random.Range(0, PassedPoem.Count);
      
        poem = PassedPoem[i];
        return poem;
    }


}

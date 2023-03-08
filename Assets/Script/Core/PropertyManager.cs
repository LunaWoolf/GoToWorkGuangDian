using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertyManager : MonoSingleton<PropertyManager>
{
    [Header("Player Info")]
    [SerializeField]
    public string player_name;
    
    [SerializeField] public int society_Artistic_Freedom;

    [SerializeField] public int personal_awakening;

    public List<string[]> PassedPoem = new List<string[]>();
    public List<string[]> DeniedPoem = new List<string[]>();

    [Header("Saved Process")]
    public bool hasShownWorkTutorial = false;
    public bool hasShownNewsTutorial = false;

    [Header("Unlock Writing")]
    public bool  bReflection= false;
    public bool  bAesthetic = false;
    public bool  bRebellious = false;

    public string[] GetRandomPassedPoem()
    {
        string[] poem;
        int i = UnityEngine.Random.Range(0, PassedPoem.Count);
        poem = PassedPoem[i];
        return poem;
    }
}

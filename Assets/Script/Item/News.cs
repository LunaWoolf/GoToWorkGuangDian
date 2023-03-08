using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
[CreateAssetMenu(fileName = "News", menuName = "ScriptableObjects/News", order = 1)]
public class News : ScriptableObject
{
    [Header("Basic")]
    public int id;
    public int priority;

    [Header("Content")]
    [TextArea(5, 20)]
    public string title;
    [TextArea(5, 20)]
    public string content;
    public Image Art;

    [System.Serializable]
    public struct restrication
    {
        public string property;
        public int max;
        public int min;
        public bool istrue;
    }

    [Header("Requirement")]
    [SerializeField]public List<restrication> restricationList = new List<restrication>();

}

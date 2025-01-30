using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewPoemFile", menuName = "Poem/PoemFile")]
public class PoemFile : ScriptableObject
{
    [TextArea(10, 10)] // The first parameter is the minimum, the second is the maximum number of lines
    public string poemText;
    public string GenericReply;

    [System.Serializable]
    public struct ReviseEvent
    {
        public string ReviseWord;
        public bool canBeRevised;
        public Texture SpeicalImage;
        [TextArea(2, 2)]
        public string SpeicalReply;
        public string DialogueIDToLoad;
    }

    // This list will now show up in the inspector with the ReviseEvent struct
    [SerializeField]
    public List<ReviseEvent> ReviseEventList = new List<ReviseEvent>();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poem : MonoBehaviour
{
    public List<PoemLine> poemLines = new List<PoemLine>();

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // if the whole poem is check

    public bool CheckifPoemAllChcked()
    {
      
        foreach (PoemLine line in poemLines)
        {
            if (!line.checkBox.GetIsCheck())
            {
                return false;
            }

        }

        
        return true;

        // turn on work canvas pass butoon
    }
}

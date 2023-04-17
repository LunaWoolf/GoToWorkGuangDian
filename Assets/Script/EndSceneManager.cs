using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSceneManager : MonoBehaviour
{

    [Header("Prefab Reference")]
    public GameObject PoemLine0;
    public GameObject PoemLine1;
    public GameObject PoemLine2;
    public GameObject PoemLine3;
    public GameObject PoemLine4;

    // Start is called before the first frame update
    void Start()
    {
        PoemLine0.GetComponent<PoemLine>().SetLine(PropertyManager.instance.writeLines[0]);
        PoemLine1.GetComponent<PoemLine>().SetLine(PropertyManager.instance.writeLines[1]);
        PoemLine2.GetComponent<PoemLine>().SetLine(PropertyManager.instance.writeLines[2]);
        PoemLine3.GetComponent<PoemLine>().SetLine(PropertyManager.instance.writeLines[3]);
        PoemLine4.GetComponent<PoemLine>().SetLine(PropertyManager.instance.writeLines[4]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

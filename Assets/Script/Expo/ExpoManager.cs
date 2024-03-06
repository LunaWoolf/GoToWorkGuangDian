using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpoManager : MonoSingleton<ExpoManager>
{
    public enum ExpoState
    {
        NoText,
        FocusText,
        RevieceFocsText,
        FinalState,

    }

    [SerializeField] public ExpoState CurrentExpoState;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PlaceExpoPoemLine()
    { 
    
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}

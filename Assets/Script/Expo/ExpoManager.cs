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
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CurrentExpoState = ExpoState.NoText;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CurrentExpoState = ExpoState.FocusText;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            CurrentExpoState = ExpoState.RevieceFocsText;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            CurrentExpoState = ExpoState.FinalState;
        }

    }
}

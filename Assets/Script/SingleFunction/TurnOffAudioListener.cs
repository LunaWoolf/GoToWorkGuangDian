using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffAudioListener : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Check if there is an AudioListener in the scene
        AudioListener[] audioListener = FindObjectsOfType<AudioListener>();
        if (audioListener.Length > 1)
        {
            // Turn off the AudioListener before loading the new scene
            this.GetComponent<AudioListener>().enabled = false;
        }
        else
        {
            // Turn off the AudioListener before loading the new scene
            this.GetComponent<AudioListener>().enabled = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

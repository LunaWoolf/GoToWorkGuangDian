using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOnBeginPlay : MonoBehaviour
{
   
    void Awake()
    {
        int childCount = this.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = this.transform.GetChild(i);
            child.gameObject.SetActive(true); // Enable the child object

            // Recursively enable child objects of the child
            
        }

    }
    private void Start()
    {
     

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

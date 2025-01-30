using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScreamWordBullet : MonoBehaviour
{
    public Vector3 direction;
    public float flySpeed = 0.2f;
    public float scaleUpSpeed = 0.01f;
    float scale;
    public bool isActive = false;
    public TextMeshPro tm;



    // Start is called before the first frame update
    void Start()
    {
        ResetScreamWordBullet();
     
    }

    public void ResetScreamWordBullet()
    {
        scale = 0.1f;
        this.transform.localScale = new Vector3(scale, scale, scale);
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            scale += scaleUpSpeed;
            this.transform.localScale = new Vector3(scale, scale, scale);
            this.transform.position += flySpeed * Time.deltaTime * direction;
        }
    }

    public void SetText(string text)
    {
      
        tm.text = text;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class MessageBlockController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI NewsTitle;
    [SerializeField] TextMeshProUGUI NewsBody;
    [SerializeField] Image BackgroundImage;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SelfDestroy());
    }

    // Update is called once per frame
    void Update()
    {
        //this.transform.position = new Vector3(this.transform.position.x + Random.Range(-0.5f, 0.5f), this.transform.position.y + Random.Range(-0.5f, 0.5f), 0);
        BackgroundImage.color += new Color(Random.Range(-0.01f, 0.01f), Random.Range(-0.01f, 0.01f), Random.Range(-0.01f, 0.01f));
    }

    public void SetMessage(News n)
    {
        NewsTitle.text = n.title;
        NewsBody.text = n.content;

    }

    IEnumerator SelfDestroy()
    {
        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);
    }
}

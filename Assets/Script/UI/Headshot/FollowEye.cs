using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FollowEye : MonoBehaviour
{
    [SerializeField] GameObject pupil;
   

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;

        Vector3 ScreenmousePos = Camera.main.ScreenToWorldPoint(mousePos);

        //float xMin = this.GetComponent<RectTransform>().rect.center.x - this.GetComponent<RectTransform>().rect.width * 2;
        //float xMax = this.GetComponent<RectTransform>().rect.center.x + this.GetComponent<RectTransform>().rect.width * 2;
        //float yMin = this.GetComponent<RectTransform>().rect.center.y - this.GetComponent<RectTransform>().rect.height * 2;
        //float yMax = this.GetComponent<RectTransform>().rect.center.y + this.GetComponent<RectTransform>().rect.height * 2;


        Vector3[] v = new Vector3[4];
        this.GetComponent<RectTransform>().GetWorldCorners(v);

        float mostLeftCorner = float.MaxValue;
        float mostRightCorner = float.MinValue;
        float mostTopCorner = float.MaxValue;
        float mostBottomCorner = float.MinValue;
        foreach (var pos in v)
        {
            mostLeftCorner = Mathf.Min(mostLeftCorner, pos.x);
            mostRightCorner = Mathf.Max(mostRightCorner, pos.x);
            mostTopCorner = Mathf.Min(mostTopCorner, pos.y);
            mostBottomCorner = Mathf.Max(mostBottomCorner, pos.y);
        }


        Vector3 pupilPosition = new Vector3(Mathf.Clamp(ScreenmousePos.x, mostLeftCorner, mostRightCorner),
                                            Mathf.Clamp(ScreenmousePos.y, mostTopCorner, mostBottomCorner),
                                            0);

        pupil.GetComponent<RectTransform>().position = pupilPosition;
    }
}

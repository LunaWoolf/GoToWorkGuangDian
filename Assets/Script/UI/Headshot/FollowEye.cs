using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FollowEye : MonoBehaviour
{
    [SerializeField] GameObject pupil;
    [SerializeField] float moveSpeed;
    public Camera cam;
    public Canvas canvas;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;

        Vector3 ScreenmousePos = Camera.main.ScreenToWorldPoint(mousePos);

      
        Vector3[] v = new Vector3[4];
        this.GetComponent<RectTransform>().GetLocalCorners(v); // Get the corners in local space
        for (int i = 0; i < 4; i++)
        {
            v[i] = transform.TransformPoint(v[i]); // Convert the corners to world space
        }

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


        //Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(new Vector3(ScreenmousePos.x, ScreenmousePos.y, 0));

        // Convert the mouse position from screen space to world space that suit prespective canvas
        Vector3 localPoint;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out localPoint);
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out localPoint);

        Vector3 pupilTargetPosition = new Vector3(Mathf.Clamp(localPoint.x, mostLeftCorner, mostRightCorner),
                                            Mathf.Clamp(localPoint.y, mostTopCorner, mostBottomCorner),
                                            pupil.GetComponent<RectTransform>().position.z);

        Vector3 direction = (pupilTargetPosition - pupil.GetComponent<RectTransform>().position).normalized;


        Vector3 NextFramePostion = pupil.GetComponent<RectTransform>().position + moveSpeed * Time.deltaTime * direction;
        

        // pupil slow movement 
        pupil.GetComponent<RectTransform>().position = new Vector3(Mathf.Clamp(NextFramePostion.x, mostLeftCorner, mostRightCorner),
                                            Mathf.Clamp(NextFramePostion.y, mostTopCorner, mostBottomCorner),
                                           NextFramePostion.z);


        // pupil snap movement
        //pupil.GetComponent<RectTransform>().position = pupilTargetPosition;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



public class BusSceneMouseControl : MonoBehaviour
{
    public GameObject selectedObject;
    Vector3 offset;
    public Camera currentSceneCamera; // this doesn't sound great....

    public Canvas canvas;
    public float force_scale = 1000;
    public float distrance_threshold = 50f;
    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            //pointerEventData.position = Input.mousePosition;

            Vector2 localPoint = canvas.worldCamera.ScreenToViewportPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, canvas.worldCamera.nearClipPlane));


            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out localPoint);

            MessageBlockController[] messageAlive = FindObjectsByType<MessageBlockController>(FindObjectsSortMode.None);

            foreach (MessageBlockController m in messageAlive)
            {
                RectTransform rectTransform = m.gameObject.GetComponent<RectTransform>();
                Vector2 direction = (rectTransform.anchoredPosition - localPoint).normalized;
                float force = Mathf.Clamp(distrance_threshold / (Vector2.Distance(rectTransform.anchoredPosition, localPoint)), 0.01f, 1);
                m.gameObject.GetComponent<Rigidbody2D>().AddForce(direction * force * force_scale, ForceMode2D.Impulse);
            }
            /*List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, results);

            if (results.Count > 0)
            {
                foreach (RaycastResult r in results)
                {
                    if (r.gameObject.tag == "Message")
                    {
                        selectedObject = r.gameObject;
                        break;
                    }
                }



                // A UI element was clicked
                Debug.Log("UI element clicked!");
            }
            else
            {
                // No UI element was clicked
                Debug.Log("Mouse clicked, but not on a UI element.");
            }

            //Destroy(selectedObject);*/
        }

        /*
        if (selectedObject)
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out localPoint);

            // Set the position of the UI element to the position of the mouse
            selectedObject.GetComponent<RectTransform>().localPosition = localPoint;
        }


        if (Input.GetMouseButtonUp(0))
        {
            if (selectedObject)
            {
                selectedObject = null;
            }

        }*/


    }


    ///Gets all event systen raycast results of current mouse or touch position.
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }
}

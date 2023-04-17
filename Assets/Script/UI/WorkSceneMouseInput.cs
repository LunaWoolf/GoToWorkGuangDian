using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class WorkSceneMouseInput : MonoBehaviour
{
    public GameObject selectedObject;
    Vector3 offset;
    public Camera currentSceneCamera; // this doesn't sound great....

    public Canvas canvas;
    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
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

            //Destroy(selectedObject);
        }

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

        }


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

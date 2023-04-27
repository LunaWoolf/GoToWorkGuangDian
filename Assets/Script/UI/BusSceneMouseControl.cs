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

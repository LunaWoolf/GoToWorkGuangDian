using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PaperShredderMouseInput : MonoBehaviour
{
    public GameObject selectedObject;
    Vector3 offset;
    public Camera currentSceneCamera; // this doesn't sound great....
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        Vector3 mousePosition = currentSceneCamera.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            Collider2D targetObject = Physics2D.OverlapPoint(mousePosition);
            if (targetObject)
            {
                selectedObject = targetObject.transform.gameObject;
                offset = selectedObject.transform.position - mousePosition;
            }
        }
        if (selectedObject)
        {
            selectedObject.transform.position = mousePosition + offset;
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (selectedObject)
            {
                
                PoemPaperController ppController = IsPointerOverPoemPaper();
                if (ppController != null)
                {
                    if (selectedObject.GetComponentInParent<ShredderWord>())
                    {
                        ppController.TryAddWordToPoem(selectedObject.GetComponentInParent<ShredderWord>().word);
                    }
                }
                else if (IsPointerOverSingleWord())
                {
                    IsPointerOverSingleWord().SetText(selectedObject.GetComponentInParent<ShredderWord>().word);
                }

                selectedObject = null;
            }
               
        }

       
    }

    public static PoemPaperController IsPointerOverPoemPaper()
    {
        return IsPointerOverPoemPaper(GetEventSystemRaycastResults());
    }

    public static Word IsPointerOverSingleWord()
    {
        return IsPointerOverSingleWord(GetEventSystemRaycastResults());
    }

    ///Returns 'true' if we touched or hovering on Unity UI element.
    public static PoemPaperController IsPointerOverPoemPaper(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.tag == "PoemPaper")
            {
                
                if (curRaysastResult.gameObject.GetComponentInParent<PoemPaperController>())
                {
                    return curRaysastResult.gameObject.GetComponentInParent<PoemPaperController>();
                }
                   
                else if (curRaysastResult.gameObject.GetComponentInChildren<PoemPaperController>())
                    return curRaysastResult.gameObject.GetComponentInChildren<PoemPaperController>();
            }
               
        }
        return null;
    }

    ///Returns 'true' if we touched or hovering on Unity UI element.
    public static Word IsPointerOverSingleWord(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.GetComponentInParent<Word>())
            {
                return curRaysastResult.gameObject.GetComponentInParent<Word>();
            }

            else if (curRaysastResult.gameObject.GetComponentInChildren<Word>())
                return curRaysastResult.gameObject.GetComponentInChildren<Word>();

        }
        return null;
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

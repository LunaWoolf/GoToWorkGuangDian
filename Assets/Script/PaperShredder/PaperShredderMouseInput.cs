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
                    if (selectedObject.GetComponentInParent<ShredderWord>()) // write mode
                    {
                        ppController.TryAddWordToPoem(selectedObject.GetComponentInParent<ShredderWord>().word);
                    }
                }
                else if (IsPointerOverSingleWord())//SaySomething
                {
                    if (selectedObject.GetComponentInParent<ShredderWord>())
                    {
                        if (IsPointerOverSingleWord())
                        {
                            Word w = IsPointerOverSingleWord();
                            if (w.currentWordType != Word.WordType.Empty)
                                return;
                            string word = selectedObject.GetComponentInParent<ShredderWord>().word;
                            GameManager.GameMode currentMode = GameManager.instance.GetCurrentGameMode();
                            if (currentMode == GameManager.GameMode.SaySomething)
                            {
                                FindObjectOfType<SaySomethingManager>().ReplaceText(w, word);
                            }
                            else if (currentMode == GameManager.GameMode.Conversation)
                            {
                                FindObjectOfType<Mission>().ReplaceText(w, word);
                            }
                          
                            Destroy(selectedObject.GetComponentInParent<ShredderWord>().gameObject);
                        }
                        
                       
                    }
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
        if (eventSystemRaysastResults == null) return null;
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
        if (eventSystemRaysastResults == null) return null;
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
        if (EventSystem.current == null) return null;
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        if (eventData == null) return null;
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }
}

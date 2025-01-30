using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClickableObject : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    public UnityEvent ButtonLeftClick;
    public UnityEvent ButtonLeftHover;
    public UnityEvent ButtonRightClick;
    public UnityEvent ButtonMiddleClick;
    public bool CanUseHoveAsLeftClick;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            ButtonLeftClick.Invoke();
            Debug.Log("Left click");
        } 
        else if (eventData.button == PointerEventData.InputButton.Middle)
        {
            ButtonMiddleClick.Invoke();
            Debug.Log("Middle click");
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            ButtonRightClick.Invoke();
            Debug.Log("Right click");
        }
           
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(CanUseHoveAsLeftClick)
            ButtonLeftHover.Invoke();
    }


    
}
using UnityEngine;
using UnityEngine.Events;

public class Clickable3DObject : MonoBehaviour
{
    public UnityEvent onClick;
    bool hasClicked = false;
    private void OnMouseDown()
    {
        if (onClick != null)
        {
            if (!hasClicked)
            {
                onClick.Invoke();
                hasClicked = true;
            }
           
        }
    }
}
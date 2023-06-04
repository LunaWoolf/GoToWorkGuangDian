using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{
    [SerializeField] bool isFloating = true;
    [SerializeField] private RectTransform targetRectTransform;
    [SerializeField] private Transform targetTransform;
    [SerializeField] private float floatSpeed = 1f;
    [SerializeField] private float floatRange = 10f;
    [SerializeField] private float xSpeedMultiplier = 0.5f;
    [SerializeField] private float ySpeedMultiplier = 0.5f;
    [SerializeField] private float zSpeedMultiplier = 5f;
    private Vector2 startPos2;
    private Vector3 startPos3;
    private void Start()
    {
        targetRectTransform = this.GetComponent<RectTransform>();
        targetTransform = this.GetComponent<Transform>();
        if (targetRectTransform != null)
            startPos2 = targetRectTransform.anchoredPosition;
        else
            startPos3 = targetTransform.position;
    }

    private void Update()
    {
        if (!isFloating) return;
        // Calculate the floating effect using a sine wave
        float yOffset = Mathf.Sin(Time.time * floatSpeed * ySpeedMultiplier) * floatRange;
        float xOffset = Mathf.Sin(Time.time * floatSpeed * xSpeedMultiplier) * floatRange;
        float  zOffset = Mathf.Sin(Time.time * floatSpeed * zSpeedMultiplier) * floatRange;
      
      
        if (targetRectTransform != null)
        {
            Vector2 newPos2 = startPos2 + new Vector2(xOffset, yOffset);

            targetRectTransform.anchoredPosition = newPos2;
        }
        else
        {
            Vector3 newPos3 = startPos3 + new Vector3(xOffset, yOffset, zOffset);
            targetTransform.position = newPos3;

        }
    }

    public void SetStartPosition2D(Vector2 position)
    {
        if (targetRectTransform != null)
        {
            isFloating = false;
            startPos2 = position;
            isFloating = true;
        }
          
    }
}

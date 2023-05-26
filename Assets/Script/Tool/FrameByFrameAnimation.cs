using UnityEngine;

public class FrameByFrameAnimation : MonoBehaviour
{
    public float frameRate = 0.1f; // The time delay between each frame (in seconds)
    public bool persistFrames = false; // Whether frames should persist on screen after moving to the next frame
    public bool isPaused = false; // Whether the animation is currently paused

    private GameObject[] frames;
    private int currentFrameIndex = 0;
    private bool isAnimating = false;

    private void Start()
    {
        // Get all child objects (frames)
        frames = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            frames[i] = transform.GetChild(i).gameObject;
        }

        // Disable all frames except the first one
        for (int i = 1; i < frames.Length; i++)
        {
            frames[i].SetActive(false);
        }
    }

    private void Update()
    {
        if (!isAnimating && !isPaused)
        {
            isAnimating = true;
            Invoke("NextFrame", frameRate);
        }
    }

    private void NextFrame()
    {
        // Disable the current frame
        frames[currentFrameIndex].SetActive(!persistFrames);

        // Move to the next frame
        currentFrameIndex++;
        if (currentFrameIndex >= frames.Length)
        {
            currentFrameIndex = 0;
            if (!persistFrames)
            {
                // Reset all frames to disabled after one loop
                foreach (GameObject frame in frames)
                {
                    frame.SetActive(false);
                }
            }
        }

        // Enable the next frame
        frames[currentFrameIndex].SetActive(true);

        isAnimating = false;

        if (!isPaused)
        {
            Invoke("NextFrame", frameRate);
        }
    }
}

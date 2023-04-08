using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HeadshotManager : MonoBehaviour
{
    // Start is called before the first frame update
    [System.Serializable]
    public struct headShot
    {
        public Sprite head_sprite;
        public Vector3 left_eye_position;
        public Vector3 righ_eye_position;
    }

    [Header("Reference")]
    Image HeadImage;
    RectTransform LeftEye;
    RectTransform RightEye;

    [SerializeField]public List<headShot> headShotList = new List<headShot>();

    public void SetHeadShot(headShot head)
    {
        HeadImage.sprite = head.head_sprite;
        LeftEye.position = head.left_eye_position;
        RightEye.position = head.righ_eye_position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterController : MonoBehaviour
{
    public float h = 2.0F;
    public float v = 2.0F;
    [SerializeField]
    float yaw = 0f;
    float pitch = 0f;
    Vector3 FirstPoint;
    Vector3 SecondPoint;
    Vector3 EndPoint;
    float xAngle;
    float yAngle;
    float xAngleTemp;
    float yAngleTemp;
    bool unTouchable = false;
    bool unMoveButton = true;
    public float minClampAngle;
    public float maxClampAngle;
    public bool xAngleClamp;
    bool isAndroid, isWebgl;
    void Start()
    {
#if UNITY_STANDALONE_WIN
        isWebgl=true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
#endif

#if UNITY_ANDROID
        isAndroid = true;
#endif
    }

    // Update is called once per frame
    void Update()
    {

        if (isAndroid)
        {
            TouchController();
        }
        
        if (isWebgl)
        {
            if (!Cursor.visible)
            {
                yaw += h * Input.GetAxis("Mouse X");
                pitch -= v * Input.GetAxis("Mouse Y");
                transform.eulerAngles = new Vector3(pitch, yaw, 0);
            }
        }
    }
    void TouchController()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began && !unTouchable)
            {
                if (EventSystem.current.currentSelectedGameObject)
                {
                    unMoveButton = true;
                }
                else
                {
                    unTouchable = true;
                    unMoveButton = false;
                    FirstPoint = Input.GetTouch(0).position;
                    xAngleTemp = xAngle;
                    yAngleTemp = yAngle;
                }
            }

            if (Input.GetTouch(0).phase == TouchPhase.Moved && !unMoveButton)
            {
                SecondPoint = Input.GetTouch(0).position;
                xAngle = xAngleTemp + (SecondPoint.x - FirstPoint.x) * 180 / Screen.width;
                yAngle = yAngleTemp + (SecondPoint.y - FirstPoint.y) * -90 / Screen.height;
                yAngle = Mathf.Clamp(yAngle, minClampAngle, maxClampAngle);

                if (xAngleClamp == true)
                {
                    xAngle = Mathf.Clamp(xAngle, -15.56f, 45.6f);
                }
                this.transform.rotation = Quaternion.Euler(yAngle, xAngle, 0.0f);
            }

            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                unTouchable = false;
                unMoveButton = true;
                xAngleTemp = xAngle;
                yAngleTemp = yAngle;
            }
        }
    }
}

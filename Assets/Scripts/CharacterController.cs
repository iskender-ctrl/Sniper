using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float h = 2.0F;
    public float v = 2.0F;
    [SerializeField]
    float yaw = 0f;
    float pitch = 0f;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Cursor.visible)
        {
            yaw += h * Input.GetAxis("Mouse X");
            pitch -= v * Input.GetAxis("Mouse Y");
            transform.eulerAngles = new Vector3(pitch, yaw, 0);
        }
    }
}

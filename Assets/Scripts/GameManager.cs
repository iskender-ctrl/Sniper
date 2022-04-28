using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    bool isDesktop, isHandless, pressed;
    [SerializeField]
    GameObject desktop, handless, touchScope, scope;
    [SerializeField]
    public float zoomCamera = 60;
    [SerializeField]
    GameObject[] cameras, guns;
    FireScript fireScript;
    public List<GameObject> weaponsList = new List<GameObject>();
    private KeyCode[] keyCodes = {
         KeyCode.Alpha1,
         KeyCode.Alpha2,
         KeyCode.Alpha3,
         KeyCode.Alpha4,
     };
    void Start()
    {
        fireScript = GameObject.FindObjectOfType<FireScript>();
#if UNITY_ANDROID
        isHandless = true;
#endif

#if UNITY_STANDALONE_WIN
        isDesktop = true;
#endif

        if (isDesktop == true)
        {
            desktop.SetActive(true);
            touchScope.SetActive(false);
        }

        if (isHandless == true)
        {
            handless.SetActive(true);
        }
    }
    private void Update()
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            if (cameras[i].activeInHierarchy)
            {
                cameras[i].GetComponent<Camera>().fieldOfView = zoomCamera;
            }
        }

        if (zoomCamera < 60)
        {
            touchScope.SetActive(true);
        }
        else
        {
            touchScope.SetActive(false);
        }

        if (touchScope.activeInHierarchy == true)
        {
            for (int i = 0; i < guns.Length; i++)
            {
                guns[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < guns.Length; i++)
            {
                guns[i].SetActive(true);
            }
        }

        for (int i = 0; i < keyCodes.Length; i++)
        {
            if (Input.GetKeyDown(keyCodes[i]))
            {
                SetNewWeapon(i);
            }
        }

        if (pressed && Time.time > fireScript.nextFire)
        {
            fireScript.nextFire = Time.time + fireScript.weaponFrequency;
            fireScript.Shoot();
        }
    }
    public void Slider_Zoom(float zoom)
    {
        zoomCamera = zoom;
    }
    void SetNewWeapon(int index)
    {
        DisableAllWeapons();
        weaponsList[index].SetActive(true);

        if (weaponsList[index].activeInHierarchy)
        {
            scope.SetActive(false);
        }
    }
    void DisableAllWeapons()
    {
        for (int i = 0; i < weaponsList.Count; i++)
        {
            weaponsList[i].SetActive(false);
        }
    }
    public void ShotDown()
    {
        pressed = true;

    }
    public void ShotUp()
    {
        pressed = false;
    }
}

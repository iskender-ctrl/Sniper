using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    bool isDesktop, isHandless, pressed;
    [SerializeField]
    GameObject desktop, handless, touchScope, scope, crossHair;
    [SerializeField]
    public float zoomCamera = 60;
    [SerializeField]
    GameObject[] cameras, guns, mobileGuns;
    FireScript fireScript;
    public List<GameObject> weaponsList = new List<GameObject>();
    int currentActiveIndex = 0;
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

#if UNITY_WEBGL
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
            touchScope.SetActive(true);
        }
    }
    private void Update()
    {
        if (fireScript == null)
        {
            fireScript = GameObject.FindObjectOfType<FireScript>();
        }

        for (int i = 0; i < cameras.Length; i++)
        {
            if (cameras[i].activeInHierarchy)
            {
                cameras[i].GetComponent<Camera>().fieldOfView = zoomCamera;
            }
        }

        if (zoomCamera < 60)
        {
            crossHair.SetActive(true);

            for (int i = 0; i < guns.Length; i++)
            {
                guns[i].SetActive(false);
            }
        }
        else
        {
            crossHair.SetActive(false);

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
    public void NextButton()
    {
        /*mobileGuns[currentActiveIndex].SetActive(true);
        currentActiveIndex++;
    
        if (currentActiveIndex >= mobileGuns.Length)
        {
            currentActiveIndex = 0;
            mobileGuns[currentActiveIndex].SetActive(true);
        }*/
        for (int i = 0; i < mobileGuns.Length - 1; ++i)
        {
            //If it is active, then deactivate that gameObject
            if (mobileGuns[i].activeSelf == true)
            {
                mobileGuns[i].SetActive(false);
                //then set next in list active
                mobileGuns[++i].SetActive(true);
            }

            if (i >= mobileGuns.Length)
            {
                i = 0;
            }
        }
    }
}

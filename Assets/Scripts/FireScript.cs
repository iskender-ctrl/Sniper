using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireScript : MonoBehaviour
{
    public float nextFire = 0;
    public float weaponFrequency = 0.5f;
    float sliderZoomSize;
    [SerializeField]
    int minBullet, maxBullet;
    int clickOnMouse = 0;
    [SerializeField]
    bool autoRifle;
    [SerializeField]
    float zoom;
    float zoomSize = 60;
    [SerializeField]
    GameObject gun, scope, fireButton, touchScope, gameManager;
    [SerializeField]
    Camera mainCam;
    Button fireBtn;
    Animator anim;
    GunRecoil recoil;
    [SerializeField]
    Slider zoomSlider;
    AudioSource shotAudio;
    [SerializeField]
    AudioClip sounds;

    bool isDesktop, isHandless;
    void Start()
    {
        shotAudio = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        recoil = gun.GetComponent<GunRecoil>();
        fireBtn = fireButton.GetComponent<Button>();
#if UNITY_ANDROID
        isHandless = true;
#endif

#if UNITY_STANDALONE_WIN
        isDesktop = true;
#endif

        if (isHandless)
        {
            FireButton();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isDesktop)
        {
            ClickMouse();
        }

        if (isHandless)
        {
            scope.SetActive(false);
        }
        AnimationController();

        if (!scope.activeInHierarchy)
        {
            gun.SetActive(true);
        }
    }
    void ClickMouse()
    {
        if (!autoRifle)
        {
            if (Input.GetMouseButtonDown(0) && Time.time > nextFire)
            {
                nextFire = Time.time + weaponFrequency;
                Shoot();
            }
        }
        else
        {
            if (Input.GetMouseButton(0) && Time.time > nextFire)
            {
                nextFire = Time.time + weaponFrequency;
                Shoot();
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && !anim.GetCurrentAnimatorStateInfo(0).IsName("Shot"))
        {
            if (clickOnMouse == 0)
            {
                anim.SetBool("Aim", true);
                clickOnMouse++;
                StartCoroutine(ZoomOpen());
            }
            else if (clickOnMouse == 1)
            {
                mainCam.GetComponent<Camera>().fieldOfView -= zoom;
                zoomSize = mainCam.GetComponent<Camera>().fieldOfView;
                clickOnMouse++;
            }
            else
            {
                anim.SetBool("Aim", false);
                clickOnMouse = 0;
                StartCoroutine(ZoomClose());
            }
        }
    }
    private void OnDisable()
    {
        mainCam.GetComponent<Camera>().fieldOfView = 60;
        clickOnMouse = 0;
    }
    void Shoot()
    {
        if (minBullet < maxBullet)
        {
            if (isHandless)
            {
                sliderZoomSize = zoomSlider.value;
            }
            shotAudio.Play();
            anim.SetBool("Shot", true);
            minBullet++;
            recoil.RecoilFire();

            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.tag == "Zombie")
                {
                    print(hit.collider.tag);
                }
            }
        }
    }
    void AnimationController()
    {
        if (minBullet >= maxBullet)
        {
            anim.SetBool("Reload", true);

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Recharge"))
            {
                anim.SetBool("Shot", false);
                anim.SetBool("Aim", false);
                if (autoRifle)
                {
                    mainCam.GetComponent<Camera>().fieldOfView = 60;

                    if (isHandless)
                    {
                        zoomSlider.value = 60;
                    }
                }
                gun.SetActive(true);
                scope.SetActive(false);
            }

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Recharge") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                anim.SetBool("Reload", false);
                anim.SetBool("Aim", true);
                minBullet = 0;
                gun.SetActive(false);

                if (isDesktop)
                {
                    scope.SetActive(true);
                }

                if (isHandless)
                {
                    touchScope.SetActive(true);
                    zoomSlider.value = sliderZoomSize;
                }
            }
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Shot"))
        {
            if (!autoRifle)
            {
                mainCam.GetComponent<Camera>().fieldOfView = 60;

                if (isHandless)
                {
                    zoomSlider.value = 60;
                }
            }
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Shot") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            anim.SetBool("Shot", false);
            if (isHandless)
            {
                zoomSlider.value = sliderZoomSize;
            }
        }

        if (!autoRifle)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Aiming_Idle"))
            {
                gun.SetActive(false);
                mainCam.GetComponent<Camera>().fieldOfView = zoomSize;
            }
            else
            {
                gun.SetActive(true);
                scope.SetActive(false);
            }
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Aiming_Idle") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.05f)
        {
            gun.SetActive(false);
            if (isDesktop)
            {
                scope.SetActive(true);
            }

            if (isHandless)
            {
                touchScope.SetActive(true);
            }

            mainCam.GetComponent<Camera>().fieldOfView = zoomSize;
        }
    }
    IEnumerator ZoomOpen()
    {
        yield return new WaitForSeconds(0.25f);
        mainCam.GetComponent<Camera>().fieldOfView -= zoom;
        zoomSize = mainCam.GetComponent<Camera>().fieldOfView;

        if (autoRifle)
        {
            gun.SetActive(false);
            scope.SetActive(true);
        }
    }
    IEnumerator ZoomClose()
    {
        yield return new WaitForSeconds(0.25f);
        mainCam.GetComponent<Camera>().fieldOfView = 60;

        if (autoRifle)
        {
            gun.SetActive(true);
            scope.SetActive(false);
        }
    }
    public void FireButton()
    {
        fireBtn.onClick.AddListener(Shoot);
    }
}

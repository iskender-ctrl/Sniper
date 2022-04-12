using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireScript : MonoBehaviour
{
    public float nextFire = 0;
    public float weaponFrequency = 0.5f;
    [SerializeField]
    int minBullet, maxBullet;
    [SerializeField]
    GameObject gun, scope;
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetMouseButton(0) && Time.time > nextFire)
        {
            nextFire = Time.time + weaponFrequency;
            Shoot();
        }

        if (Input.GetMouseButton(1))
        {
            anim.SetBool("Aim", true);
        }

        AnimationController();
    }
    void Shoot()
    {
        if (minBullet < maxBullet)
        {
            anim.SetBool("Shot", true);
            minBullet += 1;
        }
    }
    void AnimationController()
    {
        if (minBullet >= maxBullet)
        {
            anim.SetBool("Reload", true);
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Recharge") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                anim.SetBool("Reload", false);
                minBullet = 0;
            }
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Shot") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
        {
            anim.SetBool("Shot", false);
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Aiming_Idle"))
        {
            gun.SetActive(false);
            scope.SetActive(true);
        }
        else
        {
            gun.SetActive(true);
            scope.SetActive(false);
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Recharge"))
        {
            anim.SetBool("Shot", false);
        }
    }
}

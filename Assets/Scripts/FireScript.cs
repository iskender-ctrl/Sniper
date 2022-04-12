using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireScript : MonoBehaviour
{
    public float nextFire = 0;
    public float weaponFrequency = 0.5f;
    [SerializeField]
    int minBullet, maxBullet;
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

        if (minBullet >= maxBullet)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Recharge") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                anim.SetBool("Reload", true);
                minBullet = 0;
            }
        }
    }
    void Shoot()
    {
        if (minBullet < maxBullet)
        {
            anim.SetBool("Shot", true);

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Shot") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
            {
                anim.SetBool("Shot", false);
                minBullet += 1;
            }
        }
    }
}

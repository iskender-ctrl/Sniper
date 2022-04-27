using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRecoil : MonoBehaviour
{
    Vector3 currentRotation, targetRotation;
    [SerializeField]
    private float recoilX, recoilY, recoilZ, snappiness, returnSpeed;
    [SerializeField]
    Vector3 firstRotation;
    // Update is called once per frame
    void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, firstRotation, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);
    }
    public void RecoilFire()
    {
        targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
    }
}

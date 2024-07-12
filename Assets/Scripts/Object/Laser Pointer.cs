using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPointer : MonoBehaviour
{
    [SerializeField]
    GameObject laser;

    [SerializeField]
    float shootDuration;

    public void Shoot() {
        StartCoroutine(ShootFlow());
    }

    IEnumerator ShootFlow() {
        laser.SetActive(true);

        yield return new WaitForSeconds(shootDuration);

        laser.SetActive(false);
    }
}

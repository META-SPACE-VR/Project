using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Display : MonoBehaviour
{
    // 모니터 켜짐 여부
    bool isOn = false;

    void Start() {
        // 상태에 맞춰서 오브젝트 활성화/비활성화
        gameObject.SetActive(isOn);
    }

    public void TurnOn() {
        isOn = true;
        gameObject.SetActive(isOn);
    }

    public void TurnOff() {
        isOn = false;
        gameObject.SetActive(isOn);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineManager : MonoBehaviour
{
    public GameObject scalpel;         // 메스
    public GameObject electrocautery;  // 전기 소작기
    public GameObject suction;         // 흡입기

    public GameObject leftArm;         // 왼쪽 팔
    public GameObject rightArm;        // 오른쪽 팔
    public GameObject centerArm;       // 중앙 팔

    void Start()
    {
        AttachTool(scalpel, leftArm);
        AttachTool(electrocautery, rightArm);
        AttachTool(suction, centerArm);
    }

    void AttachTool(GameObject tool, GameObject arm)
    {
        tool.transform.SetParent(arm.transform);
        tool.transform.localPosition = Vector3.zero; // 필요에 따라 위치 조정
        tool.transform.localRotation = Quaternion.identity; // 필요에 따라 회전 조정
    }
}
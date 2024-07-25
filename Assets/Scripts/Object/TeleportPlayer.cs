using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    public Transform teleportTarget; // 텔레포트할 위치
    public GameObject player; // 플레이어 오브젝트
    public KeyCode teleportKey = KeyCode.R; // 텔레포트 키 설정

    private bool canTeleport = false; // 텔레포트 가능 여부

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            canTeleport = true; // 플레이어가 트리거에 들어왔을 때 텔레포트 가능
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            canTeleport = false; // 플레이어가 트리거를 벗어났을 때 텔레포트 불가능
        }
    }

    void Update()
    {
        if (canTeleport && Input.GetKeyDown(teleportKey))
        {
            Teleport();
        }
    }

    void Teleport()
    {
        if (teleportTarget != null && player != null)
        {
            player.transform.position = teleportTarget.position;
        }
    }
}

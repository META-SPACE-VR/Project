using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    public Transform teleportTarget;
    public GameObject player;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.Theme.Primitives;

public class Goods : MonoBehaviour
{
    GameObject playerInRange = null;
    bool hasFood = true;

    [SerializeField]
    GameObject spaceFood; // 우주식량

    [SerializeField]
    GameObject interactionPrompt; // Interaction prompt

    void Awake() {
        interactionPrompt.SetActive(false);
    }

    void Update() {
        if(playerInRange && Input.GetKeyDown(KeyCode.E)) {
            SpawnFood();
        }
    }

    private void SpawnFood() {
        // 우주식량 스폰 
        GameObject spawnedFood = Instantiate(spaceFood, transform.position + Vector3.up * 4f, Quaternion.identity);
        Rigidbody foodRigid = spawnedFood.GetComponent<Rigidbody>();

        // 식량이 날아갈 방향 계산
        Transform playerCamera = playerInRange.transform.Find("Camera Offset").transform.Find("Main Camera");
        Vector3 direction = playerCamera.transform.position - transform.position;
        direction -= Vector3.up * direction.y;
        direction.Normalize();
        
        float xzPower = 2.5f;
        float yPower = 4f;
        Vector3 force = xzPower * direction + Vector3.up * yPower;
        foodRigid.AddForce(force, ForceMode.Impulse);

        playerInRange = null;
        interactionPrompt.SetActive(false);
        hasFood = false;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player") && hasFood) {
            playerInRange = other.gameObject;
            interactionPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Player") && hasFood) {
            playerInRange = null;
            interactionPrompt.SetActive(false);
        }
    }
}

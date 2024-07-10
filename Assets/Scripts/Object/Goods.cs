using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goods : MonoBehaviour
{

    bool hasFood = true;

    GameObject interactingPlayer = null;

    [SerializeField]
    GameObject interactionPrompt;

    [SerializeField]
    GameObject food;

    private void Update() {
        if(hasFood && interactingPlayer && Input.GetKeyDown(KeyCode.E)) {
            GiveItem();
        }
    }

    private void GiveItem() {
        GameObject spawnFood = Instantiate(food, transform.position + Vector3.up * 3f, Quaternion.identity);
        Rigidbody foodRB = spawnFood.GetComponent<Rigidbody>();

        Transform camera = interactingPlayer.transform.Find("Camera Offset").Find("Main Camera");
        Vector3 forceDirection = camera.position - foodRB.transform.position;
        forceDirection.y = 0;
        forceDirection.Normalize();

        Debug.Log(forceDirection);

        Vector3 force = 2.3f * forceDirection + Vector3.up * 7;
        foodRB.AddForce(force, ForceMode.Impulse);

        // 플레이어에게 아이템을 지급하는 로직
        hasFood = false;
        interactingPlayer = null;
        interactionPrompt.SetActive(false);
    }

    private void OnTriggerEnter(Collider other) {
        if(hasFood && other.CompareTag("Player")) {
            interactingPlayer = other.gameObject;
            interactionPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if(hasFood && other.CompareTag("Player")) {
            interactingPlayer = null;
            interactionPrompt.SetActive(false);
        }
    }
}

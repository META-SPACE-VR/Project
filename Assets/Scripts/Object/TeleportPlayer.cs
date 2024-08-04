using UnityEngine;
using TMPro;

public class TeleportPlayer : MonoBehaviour
{
    public Transform teleportTarget; // 텔레포트할 위치
    public GameObject player; // 플레이어 오브젝트
    public OVRInput.Button interactionButton = OVRInput.Button.Two; // 텔레포트 키 설정
    public GameObject interactionPrompt; // Interaction prompt 참조
    private bool isPlayerInRange = false;
    private bool canTeleport = false; // 텔레포트 가능 여부
    void Start()
    {
        if (player == null) Debug.LogError("Player is not assigned.");
        if (interactionPrompt == null) Debug.LogError("interactionPrompt is not assigned.");

        interactionPrompt.SetActive(false); // 시작 시 텍스트 비활성화
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어 발견!!!");
            isPlayerInRange = true;
            interactionPrompt.SetActive(true); // 텍스트 활성화
            canTeleport = true; // 플레이어가 트리거에 들어왔을 때 텔레포트 가능
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어 나감!!!");
            isPlayerInRange = false;
            interactionPrompt.SetActive(false); // 텍스트 비활성화
            canTeleport = false; // 플레이어가 트리거를 벗어났을 때 텔레포트 불가능
        }
    }

    void Update()
    {
        if (canTeleport && OVRInput.GetDown(interactionButton))
        {
            Debug.Log("텔레포트!!!");
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

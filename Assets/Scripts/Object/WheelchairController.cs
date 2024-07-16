using UnityEngine;
using TMPro;

public class WheelchairController : MonoBehaviour
{
    public float moveSpeed = 5f; // 속도 조정
    public Transform playerCamera;
    public GameObject player;
    public GameObject wheelchair;
    public TMP_Text interactionText; // 상호작용 텍스트 UI
    public GameObject npc; // NPC 객체
    public GameObject npcOnWheelchairModel; // 휠체어에 타고 있는 NPC 모델
    public float offsetZ = 0.5f; // 휠체어를 플레이어의 앞에 배치할 오프셋

    private bool isInteracting = false;
    private bool isPlayerInRange = false;
    private bool isNPCNearby = false; // NPC가 휠체어 근처에 있는지 확인하는 변수
    private Rigidbody wheelchairRigidbody;
    private Transform originalParent;
    private GameObject originalNPCModel; // 원래 NPC 모델 저장

    void Start()
    {
        // 모든 공개 변수가 할당되었는지 확인하는 디버그 로그
        if (playerCamera == null)
            Debug.LogError("PlayerCamera is not assigned.");
        if (player == null)
            Debug.LogError("Player is not assigned.");
        if (wheelchair == null)
            Debug.LogError("Wheelchair is not assigned.");
        if (interactionText == null)
            Debug.LogError("InteractionText is not assigned.");
        if (npc == null)
            Debug.LogError("NPC is not assigned.");
        if (npcOnWheelchairModel == null)
            Debug.LogError("NPC on Wheelchair Model is not assigned.");

        interactionText.gameObject.SetActive(false); // 상호작용 텍스트 비활성화

        wheelchairRigidbody = wheelchair.GetComponent<Rigidbody>();
        if (wheelchairRigidbody == null)
        {
            Debug.LogError("Wheelchair does not have a Rigidbody component.");
        }
        else
        {
            wheelchairRigidbody.isKinematic = true; // 기본적으로 kinematic 상태 유지
        }

        originalParent = wheelchair.transform.parent; // 휠체어의 원래 부모 객체 저장
        originalNPCModel = npc; // 원래 NPC 모델 저장
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (isInteracting)
            {
                ExitInteraction();
            }
            else
            {
                EnterInteraction();
            }
        }

        if (isInteracting)
        {
            // 플레이어의 입력에 따라 휠체어를 이동시킴
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector3 movement = playerCamera.forward * moveVertical + playerCamera.right * moveHorizontal;
            movement.y = 0; // 휠체어가 수직으로 이동하지 않도록
            player.transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            interactionText.gameObject.SetActive(true); // 상호작용 텍스트 활성화
        }
        if (other.CompareTag("NPC"))
        {
            isNPCNearby = true; // NPC가 휠체어 근처에 있는지 확인
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            interactionText.gameObject.SetActive(false); // 상호작용 텍스트 비활성화
        }
        if (other.CompareTag("NPC"))
        {
            isNPCNearby = false; // NPC가 휠체어 근처에서 벗어남
        }
    }

    public void EnterInteraction()
    {
        isInteracting = true;
        Debug.Log("상호작용 시작");

        if (wheelchairRigidbody != null)
        {
            wheelchairRigidbody.isKinematic = true; // 휠체어를 상호작용 중에도 kinematic 상태로 유지
        }

        // 휠체어를 플레이어의 자식으로 설정하여 함께 이동하도록 함
        wheelchair.transform.SetParent(player.transform);

        // 휠체어의 방향을 플레이어의 방향과 동일하게 설정
        wheelchair.transform.rotation = player.transform.rotation;

        // 휠체어를 플레이어의 앞쪽으로 배치
        Vector3 newPosition = player.transform.position + player.transform.forward * offsetZ;
        newPosition.y = wheelchair.transform.position.y; // 휠체어의 y 좌표는 유지
        wheelchair.transform.position = newPosition;

        if (isNPCNearby)
        {
            // NPC 모델을 휠체어에 타고 있는 모델로 변경
            originalNPCModel.SetActive(false);
            npcOnWheelchairModel.SetActive(true);
            npcOnWheelchairModel.transform.SetParent(wheelchair.transform);
            npcOnWheelchairModel.transform.localPosition = Vector3.zero; // 휠체어에 정확히 맞추기 위해 로컬 포지션을 0으로 설정
        }
    }

    public void ExitInteraction()
    {
        isInteracting = false;
        Debug.Log("상호작용 종료");

        if (wheelchairRigidbody != null)
        {
            wheelchairRigidbody.isKinematic = true; // 상호작용 종료 시 kinematic 상태 유지
        }

        // 휠체어를 원래 부모 객체로 되돌림
        wheelchair.transform.SetParent(originalParent);

        if (isNPCNearby)
        {
            // NPC 모델을 원래 모델로 복원
            npcOnWheelchairModel.SetActive(false);
            originalNPCModel.SetActive(true);
            originalNPCModel.transform.SetParent(null); // 원래 위치로 되돌리기 위해 부모를 해제
        }
    }
}

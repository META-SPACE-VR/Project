using UnityEngine;

public class WheelchairController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform playerCamera;
    public Transform wheelchairCamera;
    public GameObject player;
    public GameObject wheelchair;

    private bool isHeld = false;

    void Start()
    {
        // 모든 공개 변수가 할당되었는지 확인하는 디버그 로그
        if (playerCamera == null)
            Debug.LogError("PlayerCamera is not assigned.");
        if (wheelchairCamera == null)
            Debug.LogError("WheelchairCamera is not assigned.");
        if (player == null)
            Debug.LogError("Player is not assigned.");
        if (wheelchair == null)
            Debug.LogError("Wheelchair is not assigned.");
    }

    void Update()
    {
        if (isHeld)
        {
            // 플레이어의 입력에 따라 휠체어를 이동시킴
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector3 movement = playerCamera.forward * moveVertical + playerCamera.right * moveHorizontal;
            movement.y = 0; // 휠체어가 수직으로 이동하지 않도록
            transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);

            // E 키를 누르면 휠체어에서 내림
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("E key pressed, stopping control.");
                ToggleControl(false);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player is in the trigger area.");
            if (Input.GetKeyDown(KeyCode.E) && !isHeld)
            {
                Debug.Log("E key pressed, starting control.");
                ToggleControl(true);
            }
        }
    }

    private void ToggleControl(bool holding)
    {
        isHeld = holding;
        if (player != null) player.SetActive(!holding);
        if (playerCamera != null) playerCamera.gameObject.SetActive(!holding);
        if (wheelchairCamera != null) wheelchairCamera.gameObject.SetActive(holding);
        Debug.Log("Control toggled: " + holding);
    }
}

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    private GameObject inventory;  // 인벤토리 참조
    private InventoryManager inventoryManager; // 인벤토리 매니저 참조
    [SerializeField] private TextMeshProUGUI interactionText; // 상호작용 텍스트 표시
    [SerializeField] private PlayerController playerController; // 플레이어 컨트롤러 참조
    public Camera mainCamera; // 카메라 참조
    public Camera zoomItemCamera; // 아이템 자세히 보기 카메라

    public float thumbstickThreshold = 0.1f; // VR Thumbstick의 입력 감지 임계값

    private float range = 5.0f; // 상호작용 범위
    private InteractiveObject interactiveObject = null; // 현재 상호작용 할 수 있는 물체
    private GameObject putItem;  // Putable Object 에 올려놓은 아이템

    private bool isInventoryOpen = false; // 인벤토리 On/Off 상태
    private bool isItemPicked = false; // 인벤토리 내 아이템 선택 여부
    private bool isItemZoomed = false; // 픽한 아이템 자세히 보기 상태
    private bool isItemPut = false; // Putable Object 와 상호작용 여부
    private bool isFocused = false; // Zoomable Object 포커스 여부
    private int selectedItemIndex = 0; // 현재 선택중인 아이템의 인벤토리 내 인덱스

    private Vector2 previousThumbstickInput; // VR 이전 Thumbstick 입력 상태
    private bool isInputProcessed; // VR 입력이 처리되었는지 여부


    private void Start()
    {
        inventory = GameObject.Find("Inventory");
        inventoryManager = inventory.GetComponent<InventoryManager>();
        playerController = gameObject.GetComponent<PlayerController>();
        zoomItemCamera.enabled = false;
        range = 5.0f;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (isFocused)
        {
            HandleFocusInput();
        }
        else if (isInventoryOpen)
        {
            HandleInventoryInput();
        }
        else if (isItemZoomed)
        {
            HandleZoomInput();
        }
        else if (isItemPicked)
        {
            HandleItemPickedInput();
        }
        else
        {
            HandleNomalInput();
        }
    }

    // Input Handler
    private void HandleNomalInput()
    {
        Vector3 controllerPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
        Vector3 controllerForward = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch) * Vector3.forward;

        Ray ray = new Ray(controllerPosition, controllerForward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
        {
            InteractiveObject obj = hit.collider.GetComponent<InteractiveObject>();
            if (obj && (obj.Type == ObjectType.Collectable || obj.Type == ObjectType.Zoomable) && !isItemZoomed)
            {
                interactiveObject = obj;
                MouseEnter();
            }
            else
            {
                MouseExit();
            }
        }
        else
        {
            MouseExit();
        }
        
        // 물체 상호작용 (RT)
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) && interactiveObject)
        {
            if (interactiveObject.Type == ObjectType.Collectable)
            {
                CollectObject();
            }
            else if (interactiveObject.Type == ObjectType.Zoomable)
            {
                ToggleFocusOnObject();
            }
        }

        // 인벤토리 열기 (Y)
        if (OVRInput.GetDown(OVRInput.RawButton.Y))
        {
            ToggleInventory();
        }
    }

    private void HandleInventoryInput()
    {
        // 아이템 선택(LS)
        Vector2 currentThumbstickInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

        if (Mathf.Abs(currentThumbstickInput.x) > thumbstickThreshold)
        {
            if (currentThumbstickInput.x != previousThumbstickInput.x)
            {
                if (!isInputProcessed)
                {
                    if (currentThumbstickInput.x < 0)
                    {
                        SelectPreviousItem();
                    }
                    else if (currentThumbstickInput.x > 0)
                    {
                        SelectNextItem();
                    }

                    isInputProcessed = true;
                }
            }
        }
        else
        {
            isInputProcessed = false;
        }

        previousThumbstickInput = currentThumbstickInput;

        // 아이템 픽업(A)
        if (OVRInput.GetDown(OVRInput.RawButton.A))
        {
            PickSelectedItem();
        }

        // 돌아가기 (B)
        if (OVRInput.GetDown(OVRInput.RawButton.B))
        {
            CloseInventory();
        }
    }

    private void HandleItemPickedInput()
    {
        // 아이템 놓기 미리보기
        Vector3 controllerPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
        Vector3 controllerForward = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch) * Vector3.forward;

        Ray ray = new Ray(controllerPosition, controllerForward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
        {
            InteractiveObject obj = hit.collider.GetComponent<InteractiveObject>();
            if (obj && obj.Type == ObjectType.Putable)
            {
                interactiveObject = obj;
                PreviewItemPut();
            }
            else
            {
                PreviewCancel();
            }
        }
        else
        {
            PreviewCancel();
        }

        // 아이템 자세히 보기(A)
        if (OVRInput.GetDown(OVRInput.RawButton.A))
        {
            TogglePickedItemZoom();
        }

        // 아이템 내려놓기(RT)
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            if (isItemPut)
            {
                PutPickedItem();
            }
            else
            {
                DropPickedItem();
            }
        }

        // 아이템 픽업 취소(B)
        if (OVRInput.GetDown(OVRInput.RawButton.B))
        {
            DeselectItem();
        }
    }

    private void HandleZoomInput()
    {
        // 아이템 자세히 보기 취소(B)
        if (OVRInput.GetDown(OVRInput.RawButton.B))
        {
            TogglePickedItemZoom();
        }
    }

    private void HandleFocusInput()
    {
        // 자세히 보기 취소(B)
        if (OVRInput.GetDown(OVRInput.RawButton.B))
        {
            ToggleFocusOnObject();
        }
    }

    // NomalInput
    private void MouseEnter()
    {
        if (interactiveObject.Type == ObjectType.Collectable)
        {
            interactionText.text = string.Format("\"{0}\" 줍기 (E)", interactiveObject.Name);
        }
        else if (interactiveObject.Type == ObjectType.Zoomable)
        {
            interactionText.text = "상세 보기 (E)";
        }
        interactionText.gameObject.SetActive(true);
    }

    private void MouseExit() 
    {
        interactiveObject = null;
        interactionText.gameObject.SetActive(false);
    }

    private void CollectObject()
    {
        inventoryManager.AddItem(interactiveObject, interactiveObject.gameObject);
    }

    private void ToggleFocusOnObject()
    {
        isFocused = !isFocused;
        Camera focusCamera = interactiveObject.focusCamera;
        if (isFocused)
        {
            interactionText.gameObject.SetActive(false);
            focusCamera.gameObject.SetActive(true);
            focusCamera.enabled = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            playerController.EnterInteractionMode();
            mainCamera.enabled = false;
        }
        else
        {
            mainCamera.enabled = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            playerController.ExitInteractionMode();
            focusCamera.enabled = false;
            focusCamera.gameObject.SetActive(false);
        } 
    }

    // StateConvert
    private void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        if (inventoryManager.GetItemCount() == 0)
        {
            isInventoryOpen = false;
            Debug.Log("아이템이 없습니다.");
        }

        if (isInventoryOpen)
        {
            if (isItemPicked)
            {
                DeselectItem();
            }
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            playerController.EnterInteractionMode();
            SelectItem(0);
        }
        else
        {
            CloseInventory();
        }
    }

    private void CloseInventory()
    {
        isInventoryOpen = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        interactionText.gameObject.SetActive(false);
        playerController.ExitInteractionMode();
    }

    private void TogglePickedItemZoom()
    {
        isItemZoomed = !isItemZoomed;
        if (isItemZoomed)
        {
            inventoryManager.ZoomItem(selectedItemIndex);

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            playerController.EnterInteractionMode();
            mainCamera.enabled = false;
            zoomItemCamera.enabled = true;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            playerController.ExitInteractionMode();
            mainCamera.enabled = true;
            zoomItemCamera.enabled = false;

            inventoryManager.UnzoomItem();
        }
    }

    // InventoryInput
    private void SelectPreviousItem()
    {
        selectedItemIndex--;
        if (selectedItemIndex < 0)
        {
            selectedItemIndex = inventoryManager.GetItemCount() - 1;
        }
        SelectItem(selectedItemIndex);
    }

    private void SelectNextItem()
    {
        selectedItemIndex++;
        if (selectedItemIndex >= inventoryManager.GetItemCount())
        {
            selectedItemIndex = 0;
        }
        SelectItem(selectedItemIndex);
    }

    private void SelectItem(int index)
    {
        interactiveObject = inventoryManager.interactiveObjects[index];
        UpdateInteractionText();
    }

    private void UpdateInteractionText()
    {
        if (isInventoryOpen)
        {
            interactionText.text = string.Format("\"{0}\" 선택 중, 손에 들기 (E)", interactiveObject.Name);
            interactionText.gameObject.SetActive(true);
        }
    }

    private void PickSelectedItem()
    {
        inventoryManager.PickItem(selectedItemIndex);
        isItemPicked = true;
        CloseInventory();
    }

    //ItemPickedInput
    private void DropPickedItem()
    {
        Vector3 vec = transform.position + transform.forward * 2f + Vector3.up * 2.5f;
        inventoryManager.DropItem(selectedItemIndex, vec);
        int itemCount = inventoryManager.GetItemCount();
        if (itemCount == 0)
        {
            CloseInventory();
        }
        DeselectItem();
    }

    private void DeselectItem()
    {
        isItemPicked = false;
        inventoryManager.DeselectItem();
    }

    private void PreviewItemPut()
    {
        if (isItemPut)
        {
            return;
        }

        isItemPut = true;
        Transform putItemTransform = interactiveObject.transform.Find("PutItemPosition");
        InteractiveObject prefab = inventoryManager.GetItemByIndex(selectedItemIndex);
        putItem = Instantiate(prefab.gameObject, putItemTransform);
        putItem.transform.SetPositionAndRotation(putItemTransform.position, Quaternion.identity);
        putItem.name = prefab.name;
        Rigidbody rb = putItem.GetComponent<Rigidbody>();
        rb.isKinematic = true;

        putItem.SetActive(true);
        putItemTransform.gameObject.SetActive(true);

        // 손에 들고 있는 아이템 안보이게
        inventoryManager.DeselectItem();

        // 텍스트 UI
        interactionText.text = "아이템 놓기 (E)";
        interactionText.gameObject.SetActive(true);
    }

    private void PreviewCancel()
    {
        if (!isItemPut)
        {
            return;
        }

        // 프리팹 제거
        isItemPut = false;
        Destroy(putItem);

        // 아이템 다시 손에 들기
        inventoryManager.PickItem(selectedItemIndex);

        // 텍스트 UI 제거
        interactiveObject = null;
        interactionText.gameObject.SetActive(false);
    }

    private void PutPickedItem()
    {
        isItemPut = false;
        isItemPicked = false;
        inventoryManager.DestroyItem(selectedItemIndex);
        interactiveObject.UpdatePutItem(putItem);
    }
}

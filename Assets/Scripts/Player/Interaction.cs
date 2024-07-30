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

    private float range = 5.0f; // 상호작용 범위
    private InteractiveObject interactiveObject = null; // 현재 상호작용 할 수 있는 물체
    private GameObject putItem;  // Putable Object 에 올려놓은 아이템

    private bool isInventoryOpen = false; // 인벤토리 On/Off 상태
    private bool isItemPicked = false; // 인벤토리 내 아이템 선택 여부
    private bool isItemZoomed = false; // 픽한 아이템 자세히 보기 상태
    private bool isItemPut = false; // Putable Object 와 상호작용 여부
    private bool isFocused = false; // Zoomable Object 포커스 여부
    private int selectedItemIndex = 0; // 현재 선택중인 아이템의 인벤토리 내 인덱스


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
        if (Input.GetButtonDown("Button2"))
        {
            ToggleInventory();
        }

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
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
        {
            InteractiveObject obj = hit.collider.GetComponent<InteractiveObject>();
            if (obj && (obj.Type == ObjectType.Collectable || obj.Type == ObjectType.Zoomable))
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

        if (Input.GetButtonDown("Click") && interactiveObject)
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
    }

    private void HandleInventoryInput()
    {
        if (Input.GetButtonDown("Horizontal"))
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            if (horizontalInput < 0)
            {
                SelectPreviousItem();
            }
            else if (horizontalInput > 0)
            {
                SelectNextItem();
            }
        }

        if (Input.GetButtonDown("Click"))
        {
            PickSelectedItem();
        }

        if (Input.GetButtonDown("Cancel"))
        {
            CloseInventory();
        }
    }

    private void HandleItemPickedInput()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
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

        if (Input.GetButtonDown("Button1"))
        {
            TogglePickedItemZoom();
        }

        if (Input.GetButtonDown("Click"))
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

        if (Input.GetButtonDown("Cancel"))
        {
            DeselectItem();
        }
    }

    private void HandleZoomInput()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            TogglePickedItemZoom();
        }
    }

    private void HandleFocusInput()
    {
        if (Input.GetButtonDown("Cancel"))
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
        Camera focusCamera = interactiveObject.GetComponentInChildren<Camera>();
        if (isFocused)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            playerController.EnterInteractionMode();
            mainCamera.enabled = false;
            focusCamera.enabled = true;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            playerController.ExitInteractionMode();
            mainCamera.enabled = true;
            focusCamera.enabled = false;
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

        // 프리팹 미리보기 ==> 수정 사항 맞는 아이템만 미리보기 보여주기, 나머진 경고메세지 등으로 대체
        isItemPut = true;
        Transform putItemTransform = interactiveObject.transform.Find("PutItemPosition");
        InteractiveObject prefab = inventoryManager.GetItemByIndex(selectedItemIndex);
        putItem = Instantiate(prefab.gameObject, putItemTransform);
        putItem.transform.SetPositionAndRotation(putItemTransform.position, Quaternion.identity);
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

        Transform putItemTransform = interactiveObject.transform.Find("PutItemPosition");
        inventoryManager.PutItem(selectedItemIndex, putItemTransform);
        interactiveObject.UpdatePutItem(putItem);

        Destroy(putItem);
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI interactionText; // 상호작용 텍스트 표시
    [SerializeField] private Camera mainCamera; // 카메라 참조
    private GameObject inventory;  // 인벤토리 참조
    private InventoryManager inventoryManager; // 인벤토리 매니저 참조
    [SerializeField] private PlayerController playerController; // 플레이어 컨트롤러 참조

    public float range = 10.0f; // 상호작용 범위

    private InteractiveObject interactiveObject = null; // 현재 주을 수 있는 물체
    private bool isInventoryOpen = false; // 인벤토리 On/Off 상태
    private bool isItemPicked = false; // 선택된 아이템 여부
    private int selectedItemIndex = 0; // 현재 선택중인 아이템의 인벤토리 내 인덱스


    private void Start()
    {
        mainCamera = Camera.main;
        inventory = GameObject.Find("Inventory");
        inventoryManager = inventory.GetComponent<InventoryManager>();
        playerController = gameObject.GetComponent<PlayerController>();
        range = 10.0f;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Button2"))
        {
            ToggleInventory();
        }

        if (isInventoryOpen)
        {
            HandleInventoryInput();
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

    private void HandleNomalInput()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
        {
            InteractiveObject obj = hit.collider.GetComponent<InteractiveObject>();
            if (obj)
            {
                interactiveObject = obj;
                OnMouseEnter();
            }
            else
            {
                OnMouseExit();
            }
        }
        else
        {
            OnMouseExit();
        }

        if (Input.GetButtonDown("Click") && interactiveObject)
        {
            if (interactiveObject.Type == ObjectType.Collectable)
            {
                CollectObject();
            }
            else if (interactiveObject.Type == ObjectType.Zoomable)
            {
                FocusOnObject();
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
        
    }

    // NomalInput
    private void OnMouseEnter()
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

    private void OnMouseExit() 
    {
        interactiveObject = null;
        interactionText.gameObject.SetActive(false);
    }

    private void CollectObject()
    {
        inventoryManager.AddItem(interactiveObject, interactiveObject.gameObject);
    }

    private void FocusOnObject()
    {
        mainCamera.transform.LookAt(interactiveObject.transform.position);
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
        playerController.ExitInteractionMode();
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
    private void DropSelectedItem()
    {
        Vector3 vec = transform.position + transform.forward * 2f + Vector3.up * 2.5f;
        inventoryManager.DropItem(selectedItemIndex, vec);
        int itemCount = inventoryManager.GetItemCount();
        if (itemCount == 0)
        {
            CloseInventory();
        }
    }

    

    

}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    private TextMeshProUGUI interactionText;
    private GameObject inventory;
    private InventoryManager inventoryManager;
    
    public float range = 10.0f;
    public InteractiveObject interactiveObject = null;
    
    public GameObject inventoryPanel;
    private bool isInventoryOpen = false;
    private int selectedItemIndex = 0;

    private void Start()
    {
        mainCamera = Camera.main;
        interactionText = GameObject.Find("ShowText").GetComponent<TextMeshProUGUI>();
        inventory = GameObject.Find("Inventory");
        inventoryManager = inventory.GetComponent<InventoryManager>();
        range = 10.0f;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            ToggleInventory();
        }

        if (isInventoryOpen)
        {
            HandleInventoryInput();
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

        if (Input.GetButtonDown("Fire1") && interactiveObject)
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
            SelectItem(0);
        }
        else
        {
            CloseInventory();
        }
    }

    private void HandleInventoryInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            SelectPreviousItem();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            SelectNextItem();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            DropSelectedItem();
        }

        if (Input.GetButtonDown("Cancel"))
        {
            CloseInventory();
        }
    }

    private void SelectItem(int index)
    {
        interactiveObject = inventoryManager.interactiveObjects[index];
        UpdateInteractionText();
    }

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

    private void CloseInventory()
    {
        isInventoryOpen = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void UpdateInteractionText()
    {
        if (isInventoryOpen)
        {
            interactionText.text = string.Format("\"{0}\" 선택 중, 버기리 (E)", interactiveObject.Name);
            interactionText.gameObject.SetActive(true);
        }
    }

}

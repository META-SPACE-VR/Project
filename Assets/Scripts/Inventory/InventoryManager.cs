using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public List<InteractiveObject> initialItems;
    public GameObject[] slots;
    public Dictionary<int, InteractiveObject> interactiveObjects;
    private GameObject pickedItemPosition;
    private GameObject pickedItem;
    private GameObject zoomedItemPosition;
    private GameObject zoomedItem;

    private void Start()
    {
        interactiveObjects = new Dictionary<int, InteractiveObject>();
        pickedItemPosition = GameObject.Find("PickedItemPosition");
        pickedItemPosition.SetActive(false);
        zoomedItemPosition = GameObject.Find("ZoomedItemPosition");
        zoomedItemPosition.SetActive(false);

        for (int i = 0; i < slots.Length; i++) 
        {
            // 초기 설정: 아이템이 있을 경우 활성화, 없을 경우 비활성화
            Image itemImage = slots[i].transform.Find("Item").GetComponent<Image>();
            if (i < initialItems.Count)
            {
                InteractiveObject item = initialItems[i];
                itemImage.sprite = item.Icon;
                itemImage.enabled = true;
                interactiveObjects[i] = item;
            }
            else
            {
                itemImage.sprite = null;
                itemImage.enabled = false;
            }
        }
    }

    public void AddItem(InteractiveObject item, GameObject obj)
    {
        bool itemAdded = false;

        for (int i = 0;i < slots.Length;i++)
        {
            Image itemImage = slots[i].transform.Find ("Item").GetComponent<Image>();
            if(itemImage.sprite == null)
            {
                itemImage.sprite = item.Icon;
                interactiveObjects[i] = item;
                itemImage.enabled = true;
                itemAdded = true;
                break;
            }
        }

        if (itemAdded)
        {
            Debug.Log("아이템 추가 완료.");
            obj.SetActive(false);
        }
        else
        {
            Debug.Log("아이템 창이 가득 찼습니다.");
        }
    }

    public void RemoveItem(int index)
    {
        if (index >= 0 && index < slots.Length)
        {
            Image itemImage = slots[index].transform.Find("Item").GetComponent<Image>();
            itemImage.sprite = null;
            itemImage.enabled = false;

            if (interactiveObjects.ContainsKey(index))
            {
                interactiveObjects.Remove(index);
            }

            for (int i = index; i < slots.Length - 1; i++)
            {
                Image currentItemImage = slots[i].transform.Find("Item").GetComponent<Image>();
                Image nextItemImage = slots[i + 1].transform.Find("Item").GetComponent<Image>();

                if (nextItemImage.sprite != null)
                {
                    currentItemImage.sprite = nextItemImage.sprite;
                    currentItemImage.enabled = true;
                    
                    if (interactiveObjects.ContainsKey(i + 1))
                    {
                        interactiveObjects[i] = interactiveObjects[i + 1];
                    }
                    else
                    {
                        interactiveObjects.Remove(i);
                        break;
                    }
                }
                else
                {
                    currentItemImage.sprite = null;
                    currentItemImage.enabled = false;
                    interactiveObjects.Remove(i);
                    break;
                }
            }

            Image lastItemImage = slots[^1].transform.Find("Item").GetComponent<Image>();
            lastItemImage.sprite = null;
            lastItemImage.enabled = false;
            interactiveObjects.Remove(slots.Length - 1);
        }
    }

    public void PickItem(int index)
    {
        if (index < 0 || index >= slots.Length)
        {
            Debug.LogError("Index Error: Inventory Manager Pick Item Index Error.");
            return;
        }

        InteractiveObject prefab = interactiveObjects[index];

        if (prefab == null)
        {
            Debug.LogError("Reference Error: Inventory Manager Pick Item Prefab is Null.");
            return;
        }

        if (pickedItem != null) // 이미 있다면 삭제
        {
            Destroy(pickedItem);
        }

        pickedItem = Instantiate(prefab.gameObject, pickedItemPosition.transform);  // 생성
        pickedItem.transform.position = pickedItemPosition.transform.position;      // 위치 조정
        pickedItem.transform.rotation = Quaternion.identity;
        pickedItem.name = "pickedItem";                                             // 이름 지정
        Rigidbody rb = pickedItem.GetComponent<Rigidbody>();                        // 기타 속성 제거
        Destroy(rb);
        CollectableObject collectable = pickedItem.GetComponent<CollectableObject>();
        Destroy(collectable);
        Collider[] colliders = pickedItem.GetComponents<Collider>();
        foreach (Collider collider in colliders)
        {
            Destroy(collider);
        }

        pickedItem.SetActive(true);
        pickedItemPosition.SetActive(true);
    }

    public void DeselectItem()
    {
        Destroy(pickedItem);
        pickedItemPosition.SetActive(false);
    }

    public void DropItem(int index, Vector3 dropPosition)
    {
        if (index >= 0 && index < slots.Length)
        {
            InteractiveObject dropItem = interactiveObjects[index];
            Image itemImage = slots[index].transform.Find("Item").GetComponent<Image>();
            
            if (itemImage.sprite != null && dropItem != null)
            {
                dropItem.gameObject.SetActive(true);

                dropItem.transform.position = dropPosition;

                RemoveItem(index);
            }
        }
    }

    public void ZoomItem(int index)
    {
        if (index < 0 || index >= slots.Length)
        {
            Debug.LogError("Index Error: Inventory Manager Zoom Item Index Error.");
            return;
        }

        InteractiveObject prefab = interactiveObjects[index];

        if (prefab == null)
        {
            Debug.LogError("Reference Error: Inventory Manager Zoom Item Prefab is Null.");
            return;
        }

        if (zoomedItem != null)
        {
            Destroy(zoomedItem);
        }

        zoomedItem = Instantiate(prefab.gameObject, zoomedItemPosition.transform);
        zoomedItem.transform.position = zoomedItemPosition.transform.position;
        zoomedItem.transform.rotation = Quaternion.identity;
        zoomedItem.name = "zoomedItem";
        Rigidbody rb = zoomedItem.GetComponent<Rigidbody>();                        // 기타 속성 제거, 변경
        Destroy(rb);
        Collider[] colliders = zoomedItem.GetComponents<Collider>();
        foreach (Collider collider in colliders)
        {
            Destroy(collider);
        }
        CollectableObject collectable = zoomedItem.GetComponent<CollectableObject>();

        zoomedItem.SetActive(true);
        zoomedItemPosition.SetActive(true);
        collectable.Type = ObjectType.Zoomed;
    }

    public void UnzoomItem()
    {
        Destroy(zoomedItem);
        zoomedItemPosition.SetActive(false);
    }

    public int GetItemCount()
    {
        return interactiveObjects.Count;
    }
}

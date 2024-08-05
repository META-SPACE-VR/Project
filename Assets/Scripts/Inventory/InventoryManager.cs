using Oculus.Interaction;
using Oculus.Interaction.Surfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public List<Collectable> initialItems;
    public GameObject[] slots;
    public Dictionary<int, Collectable> collectables;
    private GameObject pickedItemPosition;
    private GameObject pickedItem;
    private GameObject zoomedItemPosition;
    private GameObject zoomedItem;

    private void Start()
    {
        collectables = new Dictionary<int, Collectable>();
        pickedItemPosition = GameObject.Find("PickedItemPosition");
        pickedItemPosition.SetActive(false);
        zoomedItemPosition = GameObject.Find("ZoomedItemPosition");
        zoomedItemPosition.SetActive(false);

        for (int i = 0; i < slots.Length; i++) 
        {
            Image itemImage = slots[i].transform.Find("Item").GetComponent<Image>();
            if (i < initialItems.Count)
            {
                Collectable item = initialItems[i];
                itemImage.sprite = item.Icon;
                itemImage.enabled = true;
                collectables[i] = item;
            }
            else
            {
                itemImage.sprite = null;
                itemImage.enabled = false;
            }
        }
    }

    public void AddItem(Collectable item, GameObject obj)
    {
        bool itemAdded = false;

        for (int i = 0;i < slots.Length;i++)
        {
            Image itemImage = slots[i].transform.Find ("Item").GetComponent<Image>();
            if(itemImage.sprite == null)
            {
                itemImage.sprite = item.Icon;
                collectables[i] = item;
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

            if (collectables.ContainsKey(index))
            {
                collectables.Remove(index);
            }

            for (int i = index; i < slots.Length - 1; i++)
            {
                Image currentItemImage = slots[i].transform.Find("Item").GetComponent<Image>();
                Image nextItemImage = slots[i + 1].transform.Find("Item").GetComponent<Image>();

                if (nextItemImage.sprite != null)
                {
                    currentItemImage.sprite = nextItemImage.sprite;
                    currentItemImage.enabled = true;
                    
                    if (collectables.ContainsKey(i + 1))
                    {
                        collectables[i] = collectables[i + 1];
                    }
                    else
                    {
                        collectables.Remove(i);
                        break;
                    }
                }
                else
                {
                    currentItemImage.sprite = null;
                    currentItemImage.enabled = false;
                    collectables.Remove(i);
                    break;
                }
            }

            Image lastItemImage = slots[^1].transform.Find("Item").GetComponent<Image>();
            lastItemImage.sprite = null;
            lastItemImage.enabled = false;
            collectables.Remove(slots.Length - 1);
        }
    }

    public void RemoveItemByName(string itemName)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            Image itemImage = slots[i].transform.Find("Item").GetComponent<Image>();
            if (itemImage.sprite != null && itemImage.sprite.name == itemName)
            {
                itemImage.sprite = null;
                itemImage.enabled = false;
                if (collectables.ContainsKey(i))
                {
                    collectables.Remove(i);
                }
                break;
            }
        }
    }

    public void PickItem(int index)
    {
        if (index < 0 || index >= slots.Length)
        {
            Debug.LogError("Index Error: Inventory Manager Pick Item Index Error.");
            return;
        }

        Collectable prefab = collectables[index];

        if (prefab == null)
        {
            Debug.LogError("Reference Error: Inventory Manager Pick Item Prefab is Null.");
            return;
        }

        if (pickedItem != null)
        {
            Destroy(pickedItem);
        }

        pickedItem = Instantiate(prefab.gameObject, pickedItemPosition.transform);
        pickedItem.transform.SetPositionAndRotation(pickedItemPosition.transform.position, Quaternion.identity);
        Rigidbody rb = pickedItem.GetComponent<Rigidbody>();
        InteractableUnityEventWrapper IUEW = pickedItem.GetComponent<InteractableUnityEventWrapper>();
        ColliderSurface colliderSurface = pickedItem.GetComponent<ColliderSurface>();
        RayInteractable rayInteractable = pickedItem.GetComponent<RayInteractable>();
        Collider[] colliders = pickedItem.GetComponents<Collider>();
        Destroy(rb);
        Destroy(IUEW);
        Destroy(colliderSurface);
        Destroy(rayInteractable);
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
            Collectable dropItem = collectables[index];
            
            if (dropItem != null)
            {
                dropItem.gameObject.SetActive(true);

                dropItem.transform.position = dropPosition;
                Rigidbody rb = dropItem.GetComponent<Rigidbody>();
                rb.isKinematic = false;

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

        Collectable prefab = collectables[index];

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
        zoomedItem.transform.SetPositionAndRotation(zoomedItemPosition.transform.position, Quaternion.identity);
        zoomedItem.name = "zoomedItem";
        Rigidbody rb = zoomedItem.GetComponent<Rigidbody>();                      
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

    public void DestroyItem(int index)
    {
        if (index >= 0 && index < slots.Length)
        {
            Collectable item = collectables[index];

            if (item != null)
            {   
                Destroy(item.gameObject);

                RemoveItem(index);
            }
        }
    }

    public Collectable GetItemByIndex(int index)
    {
        return collectables[index];
    }

    public int GetItemCount()
    {
        return collectables.Count;
    }
}

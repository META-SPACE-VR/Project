using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public List<InteractiveObject> initialItems;
    public GameObject[] slots;
    public Dictionary<int, InteractiveObject> interactiveObjects;

    private void Start()
    {
        interactiveObjects = new Dictionary<int, InteractiveObject>();

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

    public int GetItemCount()
    {
        return interactiveObjects.Count;
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public GameObject Prefab;
    public string Name;
    public Sprite Icon;
    public TextMeshProUGUI guideText;
    public InventoryManager InventoryManager;

    public void ShowText()
    {
        guideText.text = string.Format("\"{0}\" 줍기", Name);
    }

    public void HideText()
    {
        guideText.text = "";
    }

    public void Collect()
    {
        InventoryManager.AddItem(this, gameObject);
    }
}

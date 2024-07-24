using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum ObjectType
{
    Collectable,
    Zoomable,
};

public class InteractiveObject : MonoBehaviour
{
    public GameObject Prefab;
    public ObjectType Type;
    public string Name;
    public Sprite Icon;

    private void Awake()
    {
        Prefab = gameObject;
    }
}

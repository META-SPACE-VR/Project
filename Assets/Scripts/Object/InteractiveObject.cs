using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum ObjectType
{
    Collectable,
    Putable,
    Zoomable,
    Zoomed
};

public class InteractiveObject : MonoBehaviour
{
    public GameObject Prefab;
    public ObjectType Type;
    public string Name;
    public Sprite Icon;

    public float rotationSpeed = 500.0f;

    private void Awake()
    {
        Prefab = gameObject;
    }

    private void Update()
    {
        if (Type == ObjectType.Zoomed)
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            transform.Rotate(Vector3.down, mouseX * rotationSpeed * Time.deltaTime, Space.World);
            transform.Rotate(Vector3.right, mouseY * rotationSpeed * Time.deltaTime, Space.World);
        }
    }
}

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
    // Common
    public GameObject Prefab;
    public ObjectType Type;
    public string Name;
    public Sprite Icon;

    // Zoomable
    public Camera focusCamera;

    // Zoomed
    private float rotationSpeed = 500.0f;

    // Putable
    public GameObject putItem;

    private void Awake()
    {
        Prefab = gameObject;
    }

    private void Update()
    {
        if (Type == ObjectType.Zoomed)
        {
            ZoomedController();
        }
    }

    private void ZoomedController()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.Rotate(Vector3.down, mouseX * rotationSpeed * Time.deltaTime, Space.World);
        transform.Rotate(Vector3.right, mouseY * rotationSpeed * Time.deltaTime, Space.World);
    }

    public void UpdatePutItem(GameObject obj)
    {
        putItem = obj;
    }
}

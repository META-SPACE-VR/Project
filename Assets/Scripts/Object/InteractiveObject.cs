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
    public Transform rightController;
    private float rotationSpeed = 500.0f;
    private bool isHolding = false;
    private Vector3 previousRightPosition;

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
        bool rightTriggerPressed = OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger);
        bool rightGripPressed = OVRInput.Get(OVRInput.Button.SecondaryHandTrigger);

        isHolding = (rightTriggerPressed && rightGripPressed);

        if (isHolding)
        {
            Vector3 currentRightPosition = rightController.position;

            Vector3 rightMovement = currentRightPosition - previousRightPosition;

            Vector3 rotationAxis = rightMovement.normalized;
            float rotationAngle = Vector3.Magnitude(rightMovement) * rotationSpeed;

            transform.Rotate(rotationAxis, rotationAngle, Space.World);

            previousRightPosition = currentRightPosition;
        }
    }

    public void UpdatePutItem(GameObject obj)
    {
        putItem = obj;
    }
}

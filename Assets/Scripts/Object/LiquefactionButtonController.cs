using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquefactionButtonController : MonoBehaviour
{
    public Camera focusCamera;
    public LiquefactionDoorController door;
    public DialController pressureDial;
    public DialController temperatureDial;
    public InteractiveObject input;
    public Renderer Alert;
    public GameObject ingredient;
    public GameObject result;

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            Vector3 controllerPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
            Vector3 controllerForward = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch) * Vector3.forward;

            Ray ray = new Ray(controllerPosition, controllerForward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform)
                {
                    CheckValidate();
                }
            }
        }
    }

    private void CheckValidate()
    {
        if (door.isClosed && pressureDial.currentNumber == 5 && temperatureDial.currentNumber == 6 && input.putItem.name == "Oxygen")
        {
            Alert.material.color = Color.green;
            ingredient.SetActive(false);
            result.SetActive(true);
        }
        else
        {
            Alert.material.color = Color.red;
        }
    }
}

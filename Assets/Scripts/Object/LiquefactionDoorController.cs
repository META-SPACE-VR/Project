using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquefactionDoorController : MonoBehaviour
{
    public bool isClosed = false;

    private readonly float range = 5.0f;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) && Camera.main != null)
        {
            Vector3 controllerPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
            Vector3 controllerForward = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch) * Vector3.forward;

            Ray ray = new Ray(controllerPosition, controllerForward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, range))
            {
                if (hit.transform == transform)
                {
                    ToggleDoor();
                }
            }
        }
    }

    private void ToggleDoor()
    {
        isClosed = !isClosed;
        if (isClosed)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 100, 0);
        }
    }

}

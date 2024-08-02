using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialController : MonoBehaviour
{
    public int currentNumber = 4;
    public int startNumber = 4;
    public int endNumber = 7;
    public float rotationAngle = 45.0f;
    public Camera focusCamera;

    private void Update()
    {
        if (Input.GetButtonDown("Click"))
        {
            Ray ray = focusCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform)
                {
                    RotateDial();
                }
            }
        }
    }

    private void RotateDial()
    {
        currentNumber += 1;
        if (currentNumber > endNumber)
        {
            currentNumber = 1;
            transform.Rotate(0, -rotationAngle * (endNumber - 1), 0);
        }
        else
        {
            transform.Rotate(0, rotationAngle, 0);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquefactionDoorController : MonoBehaviour
{
    public bool isClosed = false;

    private readonly float range = 5.0f;

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixerCoverController : MonoBehaviour
{
    public bool isClosed = false;
    public Transform leftCover;
    public Transform rightCover;
    public Vector3 distance = new Vector3(0, 0, -0.5f);

    private readonly float range = 5.0f;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Click") && Camera.main != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, range))
            {
                if (hit.transform == leftCover || hit.transform == rightCover)
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
            leftCover.position += distance;
            rightCover.position -= distance;
        }
        else
        {
            leftCover.position -= distance;
            rightCover.position += distance;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixerCoverController : MonoBehaviour
{
    public bool isClosed = false;
    public Transform leftCover;
    public Transform rightCover;
    public float distance = 4.5f;

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
            // 왼쪽, 오른쪽 문 이동
            leftCover.Translate(distance, 0, 0);
            rightCover.Translate(-distance, 0, 0);
        }
        else
        {
            // 왼쪽, 오른쪽 문 이동
            leftCover.Translate(-distance, 0, 0);
            rightCover.Translate(distance, 0, 0);

        }
    }
}

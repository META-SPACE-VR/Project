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
        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = focusCamera.ScreenPointToRay(Input.mousePosition);
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
            // �Ķ��� => ��ȭ ��ҷ� ġȯ
            Alert.material.color = Color.green;
            ingredient.SetActive(false);
            result.SetActive(true);
        }
        else
        {
            // ������ => �״�� ��ȯ
            Alert.material.color = Color.red;
        }
    }
}

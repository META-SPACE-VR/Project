using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixerButtonController : MonoBehaviour
{
    public InteractiveObject leftInput;
    public InteractiveObject rightInput;
    public Transform leftTransform;
    public Transform rightTransform;
    public GameObject Kerosene;
    public GameObject RocketFuel;
    public MixerCoverController cover;
    public Renderer Alert;
    public Camera focusCamera;

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
        if (cover.isClosed)
        {
            if ((leftInput.putItem.name == "Naphthalene" && rightInput.putItem.name == "Dodecane") || (leftInput.putItem.name == "Dodecane" && rightInput.putItem.name == "Naphthalene"))
            {
                MixComplete(Kerosene);
            }
            else if ((leftInput.putItem.name == "Kerosene" && rightInput.putItem.name == "LOx") || (leftInput.putItem.name == "LOx" && rightInput.putItem.name == "Kerosene"))
            {
                MixComplete(RocketFuel);
            }
            else
            {
                Alert.material.color = Color.red;
            }
        }
        else
        {
            Alert.material.color = Color.red;
        }
    }

    private void MixComplete(GameObject result)
    {
        Alert.material.color = Color.green;
        leftInput.UpdatePutItem(null);
        rightInput.UpdatePutItem(null);
        if (leftTransform.childCount > 0)
        {
            Transform leftPutItem = leftTransform.GetChild(0);
            Destroy(leftPutItem);
        }
        if (rightTransform.childCount > 0)
        {
            Transform rightPutItem = leftTransform.GetChild(0);
            Destroy(rightPutItem);
        }
        result.SetActive(true);
    }
}

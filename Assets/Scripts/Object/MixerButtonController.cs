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
        if (Input.GetButtonDown("Click"))
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
        if (cover.isClosed && leftInput.putItem)
        {
            if (leftInput.putItem.name == "Naphthalene" && rightInput.putItem.name == "Dodecane")
            {
                MixComplete(Kerosene);
            }
            else if (leftInput.putItem.name == "Dodecane" && rightInput.putItem.name == "Naphthalene")
            {
                MixComplete(Kerosene);
            }
            else if (leftInput.putItem.name == "Kerosene" && rightInput.putItem.name == "LOx")
            {
                MixComplete(RocketFuel);
            }
            else if (leftInput.putItem.name == "LOx" && rightInput.putItem.name == "Kerosene")
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
        Debug.Log("Mixed");
        Alert.material.color = Color.green;

        if (leftTransform.childCount > 0)
        {
            int leftItemIndex = leftTransform.childCount - 1;
            Transform leftPutItem = leftTransform.GetChild(leftItemIndex);
            Destroy(leftPutItem.gameObject);
        }
        leftInput.UpdatePutItem(null);

        if (rightTransform.childCount > 0)
        {
            int rightItemIndex = rightTransform.childCount - 1;
            Transform rightPutItem = rightTransform.GetChild(rightItemIndex);
            Destroy(rightPutItem.gameObject);
        }
        rightInput.UpdatePutItem(null);

        result.SetActive(true);
    }
}

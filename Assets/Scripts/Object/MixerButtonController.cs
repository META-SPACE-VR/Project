using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixerButtonController : MonoBehaviour
{
    public InteractiveObject leftInput;
    public GameObject leftIngredient;
    public InteractiveObject rightInput;
    public GameObject rightIngredient;
    public CollectableObject result; 
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
        if (cover.isClosed && leftInput.putItem.name == "LOx" && rightInput.putItem.name == "Dodecane")
        {
            Alert.material.color = Color.green;
            leftIngredient.SetActive(false);
            rightIngredient.SetActive(false);
            result.name = "로켓 연료";
        }
        else
        {
            Alert.material.color = Color.red;
        }
    }
}

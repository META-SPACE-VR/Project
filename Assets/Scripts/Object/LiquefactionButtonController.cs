using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquefactionButtonController : MonoBehaviour
{
    public LiquefactionDoorController door;
    public DialController pressureDial;
    public DialController temperatureDial;
    public Putable input;
    public Renderer Alert;
    public GameObject ingredient;
    public GameObject result;

    public void CheckValidate()
    {
        Debug.Log("버튼 눌림");
        if (door.isClosed && pressureDial.currentNumber == 5 && temperatureDial.currentNumber == 6 && input.putItem.Name == "분자 7")
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

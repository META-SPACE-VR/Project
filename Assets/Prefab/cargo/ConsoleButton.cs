using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConsoleButton : MonoBehaviour
{
    [SerializeField]
    MoveType buttonMoveType;

    public UnityEvent onPress;
    public UnityEvent onRelease;
    GameObject presser;
    bool isPressed;

    [SerializeField]
    Material onMaterial;
    [SerializeField]
    Material offMaterial;

    void Start()
    {
        isPressed = false;
    }

    private void OnTriggerEnter(Collider other) {
        if(!isPressed) {
            presser = other.gameObject;
            onPress.Invoke();
            isPressed = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        Debug.Log(other.gameObject.name);
        if(other.gameObject == presser) {
            onRelease.Invoke();
            isPressed = false;
        }
    }

    public void ChangeOnMaterial(MoveType moveType) {
        if(moveType == buttonMoveType) {
            GetComponent<MeshRenderer>().material = onMaterial;
        }
    }

    public void ChangeOffMaterial() {
        GetComponent<MeshRenderer>().material = offMaterial;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineStart : MonoBehaviour
{
    public NPCInteraction npcInteraction;
    public Transform bedTransform;
    public float wheelDistance;
    public GameObject wheelchair;
    public Animator machineAnimator;

    // Update is called once per frame
    void Update()
    {
        // 'e' 키를 눌렀을 때 onMachineStart 메서드 호출
        if (Input.GetKeyDown(KeyCode.E))
        {
            onMachineStart();
        }
    }

    private bool IsNearby()
    {
        float distance = Vector3.Distance(transform.position, wheelchair.transform.position);
        return distance <= wheelDistance;
    }

    private void onMachineStart()
    {
        if (npcInteraction.isSittingInWheelchair && IsNearby())
        {
            machineAnimator.SetTrigger("Machine_Start");

            // Move the patient to the bed
            npcInteraction.LayOnBed(bedTransform);

            // Set "Laying" trigger for NPC's Animator
            npcInteraction.GetComponent<Animator>().SetTrigger("Laying");
        }
        else
        {
            Debug.Log("아직 할당 안됨");
        }
    }
}

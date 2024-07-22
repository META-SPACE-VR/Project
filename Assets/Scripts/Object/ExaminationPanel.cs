using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExaminationPanel : MonoBehaviour
{
    public Button examinationButton;
    public NPCInteraction npcInteraction;
    public Animator bedAnimator;
    public float wheelDistance;
    public GameObject wheelchair;
    public Transform bedTransform; // Add a reference to the bed's transform
    public TriggerArea triggerArea; // Add a reference to TriggerArea

    void Start()
    {
        examinationButton.onClick.AddListener(OnExaminationButtonClick);
    }

    private bool IsNearby()
    {
        float distance = Vector3.Distance(transform.position, wheelchair.transform.position);
        return distance <= wheelDistance;
    }

    private void OnExaminationButtonClick()
    {
        if (npcInteraction.isSittingInWheelchair && IsNearby())
        {
            bedAnimator.SetTrigger("Bed_Out");

            // Move the patient to the bed
            npcInteraction.LayOnBed(bedTransform);

            // Exit interaction in TriggerArea
            if (triggerArea != null)
            {
                triggerArea.ExitInteraction();
            }
        }
        else
        {
            Debug.Log("아직 할당 안됨");
        }
    }
}

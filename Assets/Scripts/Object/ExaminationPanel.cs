using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExaminationPanel : MonoBehaviour
{
    public Button examinationButton;
    public Button startExaminationButton;
    public NPCInteraction npcInteraction;
    public Animator bedAnimator;
    public float wheelDistance;
    public GameObject wheelchair;
    public Transform bedTransform; // Add a reference to the bed's transform
    public TriggerArea triggerArea; // Add a reference to TriggerArea
    public GameObject resultPanel; // Add a reference to the result panel

    void Start()
    {
        examinationButton.onClick.AddListener(OnExaminationButtonClick);
        startExaminationButton.onClick.AddListener(OnStartExaminationButtonClick);

        if (resultPanel != null)
        {
            resultPanel.SetActive(false); // Initially hide the result panel
        }

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

    private void OnStartExaminationButtonClick()
    {
        if (npcInteraction.transform.IsChildOf(bedTransform))
        {
            bedAnimator.SetTrigger("Bed_In");
            DisplayExaminationResults();
        }
    }

    private void DisplayExaminationResults()
    {
        if (resultPanel != null)
        {
            resultPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("Result panel is not assigned.");
        }
    }
}

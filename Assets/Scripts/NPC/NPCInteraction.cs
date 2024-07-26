using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class NPCInteraction : MonoBehaviour
{
    public GameObject player; // Reference to the player
    public GameObject dialoguePanel; // Reference to the dialogue UI panel
    public TMP_Text dialogueText; // Reference to the Text UI element
    public GameObject wheelchair; // Reference to the wheelchair
    public GameObject Inventory; // 인벤토리 숨기기
    public Transform sitArea; // Reference to the sit area on the wheelchair
    public Button sitWheelChairBtn; // 휠체어 태우기 버튼
    public float interactionDistance; // Distance to check if the wheelchair is nearby
    public bool isSittingInWheelchair = false;
    private bool playerNearby = false;
    private bool isInteracting = false;
    private int dialogueStep = 0;
    public Animator npcAnimator;

    private string[] dialogueLines = new string[]
    {
        "Help!",
        "I don't think I can move. Please, find something to carry me."
    };

    void Start()
    {
        sitWheelChairBtn.onClick.AddListener(sitWheelChairBtnClick);
        dialoguePanel.SetActive(false); // Initially hide the dialogue panel
        AddEventTriggerListener(dialoguePanel, EventTriggerType.PointerClick, OnDialoguePanelClick);
    }

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E) && !isInteracting)
        {
            StartDialogue();
        }
        if (IsWheelchairNearby() && playerNearby && !isSittingInWheelchair)
        {
            sitWheelChairBtn.gameObject.SetActive(true);
        }
        else
        {
            sitWheelChairBtn.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            playerNearby = true;
            Debug.Log("Player nearby");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            playerNearby = false;
            Debug.Log("Player left");
        }
    }

    private void StartDialogue()
    {
        isInteracting = true;
        dialogueStep = 0;
        dialoguePanel.SetActive(true);
        Inventory.SetActive(false);
        dialogueText.text = dialogueLines[dialogueStep];
    }

    private void AdvanceDialogue()
    {
        dialogueStep++;
        if (dialogueStep < dialogueLines.Length)
        {
            dialogueText.text = dialogueLines[dialogueStep];
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        isInteracting = false;
        dialoguePanel.SetActive(false);
        Debug.Log("Find something to carry the NPC");
    }

    private void sitWheelChairBtnClick()
    {
        if (wheelchair != null && IsWheelchairNearby())
        {
            SitInWheelchair();
        }
    }

    private bool IsWheelchairNearby()
    {
        float distance = Vector3.Distance(transform.position, wheelchair.transform.position);
        return distance <= interactionDistance;
    }

    public void SitInWheelchair()
    {
        transform.position = sitArea.position;
        transform.rotation = sitArea.rotation;
        transform.SetParent(wheelchair.transform);
        isSittingInWheelchair = true;
        npcAnimator.SetBool("Laying", false);
        sitWheelChairBtn.gameObject.SetActive(false);
        Debug.Log("NPC is now in the wheelchair.");
    }

    public void LayOnBed(Transform bedTransform)
    {
        // Move the NPC to the bed
        transform.SetParent(bedTransform);
        transform.position = bedTransform.position;
        transform.rotation = bedTransform.rotation;
        isSittingInWheelchair = false;
        npcAnimator.SetBool("Laying", true);
        Debug.Log("NPC is now on the bed.");
    }

    private void OnDialoguePanelClick(BaseEventData eventData)
    {
        if (isInteracting)
        {
            AdvanceDialogue();
        }
    }

    private void AddEventTriggerListener(GameObject obj, EventTriggerType type, UnityEngine.Events.UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = obj.AddComponent<EventTrigger>();
        }
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = type };
        entry.callback.AddListener(action);
        trigger.triggers.Add(entry);
    }
}

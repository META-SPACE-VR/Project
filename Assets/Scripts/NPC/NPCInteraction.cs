using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class NPCInteraction : MonoBehaviour
{
    public GameObject player; // Reference to the player
    public GameObject dialoguePanel; // Reference to the dialogue UI panel
    public TMP_Text dialogueText; // Reference to the Text UI element

    private bool playerNearby = false;
    private bool isInteracting = false;
    private int dialogueStep = 0;

    private string[] dialogueLines = new string[]
    {
        "Help!",
        "I don't think I can move. Please, find something to carry me."
    };

    void Start()
    {
        dialoguePanel.SetActive(false); // Initially hide the dialogue panel
        AddEventTriggerListener(dialoguePanel, EventTriggerType.PointerClick, OnDialoguePanelClick);
    }

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E) && !isInteracting)
        {
            StartDialogue();
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
        Debug.Log("Find something to carry the NPC.");
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

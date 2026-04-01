using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[System.Serializable]
public class DialogueLine
{
    public string characterName;
    [TextArea(3, 10)]
    public string text;
    public Sprite characterPortrait;
    public Sprite backgroundOverride;
    public bool triggersLevelTransition;
    public float lineSpeed = 0.08f;
}

public class DialogueController : MonoBehaviour
{
    public enum TransitionType { LoadNextRoomPrefab, LoadSpecificScene }

    [Header("Mode & Trigger Settings")]
    public bool isSignMode = false;
    public bool playOnEnter = false;

    [Header("Input Locking")]
    // We leave this hidden in the inspector now because we will find it via code
    [HideInInspector] public Movement playerMovementScript;

    [Header("UI References")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Image portraitImage;
    public Image backgroundImage;

    [Header("Sequence")]
    public List<DialogueLine> dialogueSequence = new List<DialogueLine>();

    private int index = 0;
    private bool isPlayerInRange;
    private bool isTyping;
    private bool isDialogueActive = false;
    private Coroutine currentTypewriter;

    void Start()
    {
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        FindAgnes(); // Find her immediately on start
    }

    // NEW: This method hunts for Agnes in the scene
    void FindAgnes()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerMovementScript = playerObj.GetComponent<Movement>();
        }
    }

    void Update()
    {
        if (isDialogueActive)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (isTyping) FinishLineInstantly();
                else AdvanceOrEnd();
            }
            return;
        }

        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E) && !playOnEnter)
        {
            StartDialogueSequence();
        }
    }

    private void StartDialogueSequence()
    {
        // Re-check for Agnes just in case she was spawned/moved
        if (playerMovementScript == null) FindAgnes();

        StopAllCoroutines();
        index = 0;
        isDialogueActive = true;
        dialoguePanel.SetActive(true);

        if (!isSignMode) TogglePlayerControls(false);

        StartCoroutine(DisplayLine());
    }

    void AdvanceOrEnd()
    {
        if (index < dialogueSequence.Count - 1)
        {
            index++;
            StartCoroutine(DisplayLine());
        }
        else
        {
            StartCoroutine(EndDialogueSequence());
        }
    }

    IEnumerator DisplayLine()
    {
        isTyping = true;
        DialogueLine currentLine = dialogueSequence[index];

        if (isSignMode)
        {
            if (portraitImage != null) portraitImage.gameObject.SetActive(false);
            if (backgroundImage != null) backgroundImage.gameObject.SetActive(false);
        }
        else
        {
            if (portraitImage != null)
            {
                bool hasPortrait = currentLine.characterPortrait != null;
                portraitImage.gameObject.SetActive(hasPortrait);
                if (hasPortrait) portraitImage.sprite = currentLine.characterPortrait;
            }

            if (backgroundImage != null && currentLine.backgroundOverride != null)
            {
                backgroundImage.gameObject.SetActive(true);
                backgroundImage.sprite = currentLine.backgroundOverride;
            }
        }

        if (nameText != null)
        {
            bool hasName = !string.IsNullOrEmpty(currentLine.characterName);
            nameText.gameObject.SetActive(hasName);
            if (hasName) nameText.text = currentLine.characterName;
        }

        dialogueText.text = "";

        if (currentLine.lineSpeed <= 0)
        {
            dialogueText.text = currentLine.text;
        }
        else
        {
            currentTypewriter = StartCoroutine(TypeEffect(currentLine));
            yield return currentTypewriter;
        }

        isTyping = false;
    }

    IEnumerator TypeEffect(DialogueLine line)
    {
        foreach (char letter in line.text.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(line.lineSpeed);
        }
        currentTypewriter = null;
    }

    void FinishLineInstantly()
    {
        if (currentTypewriter != null) StopCoroutine(currentTypewriter);
        dialogueText.text = dialogueSequence[index].text;
        isTyping = false;
    }

    IEnumerator EndDialogueSequence()
    {
        isDialogueActive = false;
        dialoguePanel.SetActive(false);

        if (backgroundImage != null) backgroundImage.gameObject.SetActive(false);
        if (portraitImage != null) portraitImage.gameObject.SetActive(false);

        TogglePlayerControls(true);
        yield break;
    }

    void TogglePlayerControls(bool state)
    {
        // If we still don't have Agnes, try to find her one last time
        if (playerMovementScript == null) FindAgnes();

        if (playerMovementScript != null)
        {
            // LOCKDOWN LOGIC
            playerMovementScript.enabled = state;
            playerMovementScript.isHiding = !state;

            if (playerMovementScript._rb != null)
            {
                if (!state)
                {
                    playerMovementScript._rb.linearVelocity = Vector2.zero;
                    playerMovementScript._rb.angularVelocity = 0f;
                    playerMovementScript._rb.constraints = RigidbodyConstraints2D.FreezeAll;
                }
                else
                {
                    playerMovementScript._rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                }
            }
        }
        else
        {
            Debug.LogError("DialogueController: Agnes not found! Check 'Player' Tag.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (playOnEnter && !isDialogueActive) StartDialogueSequence();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (isSignMode && isDialogueActive)
            {
                isDialogueActive = false;
                dialoguePanel.SetActive(false);
                TogglePlayerControls(true);
            }
        }
    }
}
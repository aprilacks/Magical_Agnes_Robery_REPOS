/* * =================================================================================================
 * DIALOGUE CONTROLLER: THE MASTER GUIDE
 * =================================================================================================
 * * --- PART 1: HIERARCHY SETUP (Canvas) ---
 * 1. Create a UI Canvas.
 * 2. Create a child Panel named 'DialoguePanel'. Attach your Dialogue Box sprite here.
 * 3. Inside 'DialoguePanel', create the following UI elements:
 * - TextMeshProUGUI (NameText): For the character's name.
 * - TextMeshProUGUI (DialogueText): For the main message.
 * - Image (PortraitImage): Position this where the character's face should appear.
 * - Image (BackgroundImage): Set to "Stretch/Fill" the whole screen. Move it to the TOP 
 * of the list so it stays BEHIND the text box.
 * 
 * * --- PART 2: SCRIPT ATTACHMENT ---
 * 1. Create an Empty GameObject in your scene (e.g., "Signpost" or "NPC_Yuri").
 * 2. Add a 'BoxCollider2D' or 'CircleCollider2D'. Check the [X] IS TRIGGER box.
 * 3. Attach this 'DialogueController' script to that GameObject.
 * 4. Drag your UI elements from the Hierarchy into the corresponding slots in the Inspector.
 * 
 * * --- PART 3: MODE 1 - CHARACTER MODE (DDLC Style) ---
 * 1. Ensure 'Is Sign Mode' is UNCHECKED.
 * 2. In 'Dialogue Sequence', add an element:
 * - Character Name: Type the name (e.g., "Monika").
 * - Text: Type what they say.
 * - Character Portrait: Drag in an expression sprite (e.g., 'Monika_Happy').
 * - Background Override: (Optional) Drag a room sprite to change the scene mid-chat.
 * - Line Speed: Set to 0.08 for natural talking.
 * 
 * * --- PART 4: MODE 2 - SIGN MODE (World Interaction) ---
 * 1. Ensure 'Is Sign Mode' is CHECKED.
 * 2. In 'Dialogue Sequence', add an element:
 * - Character Name: (Optional) Type the object name like "Stone Tablet". 
 * - Leave empty if you want NO name tag at all.
 * - Text: Type the sign's message.
 * - Character Portrait: (Ignored) Even if you put a sprite here, it won't show.
 * - Line Speed: Set to 0 if you want the text to appear instantly like a real sign.
 * 
 * * --- PART 5: LEVEL TRANSITIONS ---
 * 1. Make sure your 'LevelManager' is in the scene.
 * 2. On the VERY LAST line of your dialogue, check the [X] TRIGGERS LEVEL TRANSITION box.
 * 3. When that line finishes, the script will call LevelManager.Instance.LoadNextRoom().
 * =================================================================================================
 */

using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class DialogueLine
{
    public string characterName;
    [TextArea(3, 10)]
    public string text;
    public Sprite characterPortrait;
    public Sprite backgroundOverride;
    public bool triggersLevelTransition;

    [Tooltip("How fast letters appear for THIS line. 0.05 is standard, 0 is instant.")]
    public float lineSpeed = 0.05f;
}

public class DialogueController : MonoBehaviour
{
    [Header("Mode Settings")]
    public bool isSignMode = false;

    [Header("UI References")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Image portraitImage;
    public Image backgroundImage;

    [Header("Sequence")]
    public List<DialogueLine> dialogueSequence = new List<DialogueLine>();

    private int index;
    private bool isPlayerInRange;
    private bool isTyping;
    private Coroutine typingCoroutine;

    void Start()
    {
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!dialoguePanel.activeInHierarchy)
            {
                dialoguePanel.SetActive(true);
                StartDialogue();
            }
            else if (!isTyping)
            {
                NextLine();
            }
            else
            {
                FinishLineInstantly();
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(DisplayLine());
    }

    void NextLine()
    {
        if (index < dialogueSequence.Count - 1)
        {
            index++;
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            typingCoroutine = StartCoroutine(DisplayLine());
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator DisplayLine()
    {
        isTyping = true;
        DialogueLine currentLine = dialogueSequence[index];

        // --- UI Setup ---
        if (portraitImage != null)
        {
            if (isSignMode) portraitImage.gameObject.SetActive(false);
            else
            {
                bool hasPortrait = currentLine.characterPortrait != null;
                portraitImage.gameObject.SetActive(hasPortrait);
                if (hasPortrait) portraitImage.sprite = currentLine.characterPortrait;
            }
        }

        if (nameText != null)
        {
            bool hasName = !string.IsNullOrEmpty(currentLine.characterName);
            nameText.gameObject.SetActive(hasName);
            if (hasName) nameText.text = currentLine.characterName;
        }

        if (!isSignMode && backgroundImage != null && currentLine.backgroundOverride != null)
        {
            backgroundImage.sprite = currentLine.backgroundOverride;
        }

        // --- Typewriter Effect with INDEPENDENT SPEED ---
        dialogueText.text = "";

        // If speed is 0, just show the whole text immediately
        if (currentLine.lineSpeed <= 0)
        {
            dialogueText.text = currentLine.text;
        }
        else
        {
            foreach (char letter in currentLine.text.ToCharArray())
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(currentLine.lineSpeed);
            }
        }

        isTyping = false;
    }

    void FinishLineInstantly()
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        dialogueText.text = dialogueSequence[index].text;
        isTyping = false;
    }

    void EndDialogue()
    {
        bool shouldTransition = dialogueSequence[index].triggersLevelTransition;
        dialoguePanel.SetActive(false);

        if (shouldTransition && LevelManager.Instance != null)
        {
            LevelManager.Instance.LoadNextRoom();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) isPlayerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            isTyping = false;
            if (dialoguePanel != null) dialoguePanel.SetActive(false);
        }
    }
}
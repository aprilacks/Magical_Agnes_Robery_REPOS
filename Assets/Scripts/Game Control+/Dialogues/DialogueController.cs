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
    public bool triggersLevelTransition; // Now functional
    public float lineSpeed = 0.08f;
}

public class DialogueController : MonoBehaviour
{
    public enum TransitionType { LoadNextRoomPrefab, LoadSpecificScene }

    [Header("Mode & Trigger Settings")]
    public bool isSignMode = false;
    public bool playOnEnter = false;

    [Header("Transition Settings")]
    public TransitionType transitionType = TransitionType.LoadSpecificScene;
    public string sceneToLoad; // Name of the scene to load if using LoadSpecificScene

    [Header("Input Locking")]
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
        FindAgnes();
    }

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
            // Check the last line's transition bool
            bool shouldTransition = dialogueSequence[index].triggersLevelTransition;
            StartCoroutine(EndDialogueSequence(shouldTransition));
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

    IEnumerator EndDialogueSequence(bool shouldTransition)
    {
        isDialogueActive = false;
        dialoguePanel.SetActive(false);

        if (backgroundImage != null) backgroundImage.gameObject.SetActive(false);
        if (portraitImage != null) portraitImage.gameObject.SetActive(false);

        if (shouldTransition)
        {
            ExecuteTransition();
        }
        else
        {
            TogglePlayerControls(true);
        }

        yield break;
    }

    private void ExecuteTransition()
    {
        if (transitionType == TransitionType.LoadSpecificScene)
        {
            if (!string.IsNullOrEmpty(sceneToLoad))
            {
                SceneManager.LoadScene(sceneToLoad);
            }
            else
            {
                Debug.LogWarning("DialogueController: SceneToLoad is empty!");
            }
        }
        else if (transitionType == TransitionType.LoadNextRoomPrefab)
        {
            // If you have a LevelManager that handles prefab loading:
            // LevelManager.Instance.LoadNextRoom();
            Debug.Log("DialogueController: LoadNextRoomPrefab triggered (add your custom manager logic here).");
        }
    }

    void TogglePlayerControls(bool state)
    {
        if (playerMovementScript == null) FindAgnes();

        if (playerMovementScript != null)
        {
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
using UnityEngine;
using UnityEngine.InputSystem;

public class PuzzleReceiver : MonoBehaviour
{
    [Header("Puzzle Logic")]
    public string puzzleID;
    public int leversNeeded = 1;
    private int currentLeversActivated = 0;
    public bool isLocked = true;

    private bool playerInZone = false;
    private PlayerInput _playerInput;

    public void RegisterLeverActivation(string incomingID)
    {
        if (incomingID == puzzleID)
        {
            currentLeversActivated++;
            if (currentLeversActivated >= leversNeeded)
            {
                isLocked = false;
                Debug.Log("Door fully unlocked!");
            }
        }
    }

    private void Update()
    {
        // Check for the "Interact" press every frame if the player is standing at the door
        if (playerInZone && !isLocked && _playerInput != null)
        {
            if (_playerInput.actions["Interact"].WasPressedThisFrame())
            {
                TransitionToNextRoom();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
            // Cache reference once
            if (_playerInput == null) _playerInput = other.GetComponent<PlayerInput>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
        }
    }

    void TransitionToNextRoom()
    {
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.LoadNextRoom();
        }
    }
}
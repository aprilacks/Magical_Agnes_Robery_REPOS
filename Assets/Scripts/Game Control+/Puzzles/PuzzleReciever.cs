using UnityEngine;
using UnityEngine.InputSystem;

public class PuzzleReceiver : MonoBehaviour
{
    [Header("Puzzle Logic")]
    public string puzzleID;
    public int leversNeeded = 1;
    private int currentLeversActivated = 0;
    public bool isLocked = true;

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

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isLocked)
        {
            // Try to get PlayerInput if we don't have it yet
            if (_playerInput == null) _playerInput = other.GetComponent<PlayerInput>();

            // Interaction: Check the "Interact" action instead of KeyCode.E
            if (_playerInput != null && _playerInput.actions["Interact"].WasPressedThisFrame())
            {
                TransitionToNextRoom();
            }
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
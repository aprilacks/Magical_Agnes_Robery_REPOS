using UnityEngine;
using UnityEngine.InputSystem;

public class PuzzleTrigger : MonoBehaviour
{
    [Header("Connection IDs")]
    public string puzzleID;
    public string specificLeverID;

    private bool isPulled = false;
    private PlayerInput _playerInput;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isPulled)
        {
            // Cache the PlayerInput from the player entering the trigger
            if (_playerInput == null) _playerInput = other.GetComponent<PlayerInput>();

            // Use the "Interact" action mapped to A (Pro Controller) or E/H (Keyboard)
            if (_playerInput != null && _playerInput.actions["Interact"].WasPressedThisFrame())
            {
                isPulled = true;
                SendSignals();
            }
        }
    }

    void SendSignals()
    {
        // Find all Doors and tell them this puzzle ID was activated
        PuzzleReceiver[] receivers = Object.FindObjectsByType<PuzzleReceiver>(FindObjectsSortMode.None);
        foreach (var receiver in receivers)
        {
            receiver.RegisterLeverActivation(puzzleID);
        }

        // Find all Lights and tell them this specific light ID is now ON
        PuzzleLightCue[] lights = Object.FindObjectsByType<PuzzleLightCue>(FindObjectsSortMode.None);
        foreach (var light in lights)
        {
            light.ActivateLight(specificLeverID);
        }

        // Visual feedback
        if (TryGetComponent<SpriteRenderer>(out var renderer))
        {
            renderer.color = Color.gray;
        }
    }
}
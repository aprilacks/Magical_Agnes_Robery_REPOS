/* * HOW TO USE:
 * 1. Attach this to your Lever/Switch GameObject.
 * 2. Add a BoxCollider2D set to "Is Trigger" (the interaction zone).
 * 3. Set 'Puzzle ID' to match the ID on the Door (e.g., "Level1_Exit").
 * 4. Set 'Specific Lever ID' to match a unique Light ID (e.g., "L1").
 * 5. Player must be inside the trigger and press 'E' to activate.
 */

using UnityEngine;

public class PuzzleTrigger : MonoBehaviour
{
    [Header("Connection IDs")]
    public string puzzleID;
    public string specificLeverID;

    private bool isPulled = false;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E) && !isPulled)
        {
            isPulled = true;
            SendSignals();
        }
    }

    void SendSignals()
    {
        // Find all Doors and tell them this puzzle ID was activated
        PuzzleReceiver[] receivers = FindObjectsByType<PuzzleReceiver>(FindObjectsSortMode.None);
        foreach (var receiver in receivers)
        {
            receiver.RegisterLeverActivation(puzzleID);
        }

        // Find all Lights and tell them this specific light ID is now ON
        PuzzleLightCue[] lights = FindObjectsByType<PuzzleLightCue>(FindObjectsSortMode.None);
        foreach (var light in lights)
        {
            light.ActivateLight(specificLeverID);
        }

        // Visual feedback for the lever handle itself
        GetComponent<SpriteRenderer>().color = Color.gray;
    }
}
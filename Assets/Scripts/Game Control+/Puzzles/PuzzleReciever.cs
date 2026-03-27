/* * HOW TO USE:
 * 1. Attach to your Door.
 * 2. Set 'puzzleID' and 'leversNeeded'.
 * 3. When unlocked and used, it tells the LevelManager to swap the entire room prefab.
 */

using UnityEngine;

public class PuzzleReceiver : MonoBehaviour
{
    [Header("Puzzle Logic")]
    public string puzzleID;
    public int leversNeeded = 1;
    private int currentLeversActivated = 0;
    public bool isLocked = true;

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
        // Interaction: Player presses E on an unlocked door
        if (other.CompareTag("Player") && !isLocked && Input.GetKeyDown(KeyCode.E))
        {
            TransitionToNextRoom();
        }
    }

    void TransitionToNextRoom()
    {
        // Tell the LevelManager to destroy this room and load the next one
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.LoadNextRoom();
        }
    }
}
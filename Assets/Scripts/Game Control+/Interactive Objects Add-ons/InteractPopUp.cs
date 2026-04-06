using UnityEngine;

/* * HOW TO USE:
 * 1. Attach to an object the player can interact with (Chest, NPC, Door).
 * 2. Assign a World-Space Canvas or Sprite (like an "E" key icon) to 'Popup UI'.
 * 3. Ensure the GameObject has a Collider2D set to 'Is Trigger'.
 * 4. The UI will automatically enable/disable when the Player enters/exits the trigger.
 */

public class InteractPopup : MonoBehaviour
{
    [Header("Popup")]
    public GameObject popupUI;

    private void Start()
    {
        if (popupUI != null)
            popupUI.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (popupUI != null)
                popupUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (popupUI != null)
                popupUI.SetActive(false);
        }
    }
}

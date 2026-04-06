/* * HOW TO USE:
 * 1. Attach to a small Sprite (indicator light) near a door or puzzle.
 * 2. Set 'Light ID' to a unique string (e.g., "Lever_01").
 * 3. This light will turn yellow when a 'PuzzleTrigger' with the same ID is activated.
 */

using UnityEngine;

public class PuzzleLightCue : MonoBehaviour
{
    public string lightID;

    [Header("Light")]
    public Sprite spriteOff;
    public Sprite spriteOn;

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = spriteOff;
    }

    public void ActivateLight(string incomingID)
    {
        if (incomingID == lightID)
        {
            sr.sprite = spriteOn;
        }
    }
}
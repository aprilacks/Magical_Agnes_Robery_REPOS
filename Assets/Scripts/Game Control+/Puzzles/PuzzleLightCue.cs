/* * HOW TO USE:
 * 1. Create a small Sprite (Circle/Square) as a child of your Door.
 * 2. Attach this script.
 * 3. Set 'Light ID' to match the 'Specific Lever ID' on the lever (e.g., "L1").
 * 4. The script defaults to Dark Gray and turns Yellow when triggered.
 */

using UnityEngine;

public class PuzzleLightCue : MonoBehaviour
{
    public string lightID;

    [Header("Colors")]
    public Color colorOff = new Color(0.2f, 0.2f, 0.2f); // Dark Gray
    public Color colorOn = Color.yellow;

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.color = colorOff;
    }

    public void ActivateLight(string incomingID)
    {
        if (incomingID == lightID)
        {
            sr.color = colorOn;
        }
    }
}
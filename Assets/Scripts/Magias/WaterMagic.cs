using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

/* * HOW TO USE:
 * 1. Attach to the Player GameObject alongside 'Movement'.
 * 2. Setup a "Water" action in PlayerInput.
 * 3. Allows a horizontal dash that resets when the player touches the ground.
 * 4. Freezes Y-axis movement during the dash for a "straight-line" effect.
 */

public class WaterMagic : MonoBehaviour
{
    private PlayerInput _input;
    private Movement plymov;
    public Rigidbody2D agnes;
    public bool DashUsed = false;
    public float dashPower = 40f;

    void Start()
    {
        plymov = GetComponent<Movement>();
        _input = GetComponent<PlayerInput>();
    }

    void Update()
    {
        if (_input.actions["Water"].WasPressedThisFrame() && !DashUsed)
        {
            DashUsed = true;
            plymov.usingWaterMagic = true;
            float dir = plymov.ReturnDirection();
            agnes.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            plymov.SetFrameVelocity(new Vector2(dashPower * dir, 0));
            StartCoroutine(DashRoutine(dir));
        }
        if (plymov.isGrounded()) DashUsed = false;
    }

    IEnumerator DashRoutine(float direction)
    {
        yield return new WaitForSeconds(0.2f);
        plymov.SetFrameVelocity(new Vector2(-(dashPower / 3) * direction, 0));
        agnes.constraints = RigidbodyConstraints2D.FreezeRotation;
        plymov.usingWaterMagic = false;
    }
}
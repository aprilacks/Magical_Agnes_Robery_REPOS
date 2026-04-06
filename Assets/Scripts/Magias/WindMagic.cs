using UnityEngine;
using UnityEngine.InputSystem;

/* * HOW TO USE:
 * 1. Attach to the Player GameObject alongside 'Movement'.
 * 2. Setup a "Wind" action in PlayerInput.
 * 3. While in air, hold the button to glide. This reduces 'Max Fall Speed' and 'Movement Speed'.
 * 4. Automatically disables 'FireMagic' while gliding to prevent conflicts.
 */
public class WindMagic : MonoBehaviour
{
    [SerializeField] private ScriptableStats _stats;
    private PlayerInput _input;
    private WaterMagic WaterDash;
    private Movement plymov;
    private FireMagic fireExtinguisher;
    public float fallspeed;
    public float slowmo;

    // Memory variable to track if we should turn Fire back on
    private bool _fireWasEnabledBeforeGliding;
    private bool _isGliding;

    void Start()
    {
        plymov = GetComponent<Movement>();
        fireExtinguisher = GetComponent<FireMagic>();
        WaterDash = GetComponent<WaterMagic>();
        _input = GetComponent<PlayerInput>();
    }

    void Update()
    {
        if (!plymov._grounded)
        {
            if (_input.actions["Wind"].IsPressed())
            {
                // If we JUST started gliding this frame
                if (!_isGliding)
                {
                    // Store the current state of FireMagic before we mess with it
                    _fireWasEnabledBeforeGliding = fireExtinguisher.enabled;
                    _isGliding = true;
                }

                _stats.MaxFallSpeed = fallspeed;

                // Temporarily disable Fire to prevent physics conflicts
                fireExtinguisher.enabled = false;
                plymov.usingFireMagic = false;
                plymov.usingWindMagic = true;
                _stats.MaxSpeed = slowmo;

                if (_input.actions["Wind"].WasPressedThisFrame()) WaterDash.DashUsed = false;
            }
            else
            {
                StopGliding();
            }
        }
        else
        {
            StopGliding();
        }
    }

    void StopGliding()
    {
        if (_isGliding)
        {
            // Restore FireMagic to exactly what it was before we started
            fireExtinguisher.enabled = _fireWasEnabledBeforeGliding;
            _isGliding = false;
        }

        _stats.MaxFallSpeed = 40;
        plymov.usingWindMagic = false;
        _stats.MaxSpeed = 14;
    }
}
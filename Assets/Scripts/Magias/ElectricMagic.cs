using UnityEngine;
using UnityEngine.InputSystem;

/* * HOW TO USE:
 * 1. Attach to the Player GameObject alongside 'Movement'.
 * 2. Setup an "Electric" action in PlayerInput.
 * 3. Press 1: Drops a 'Marker' (the 'eletrik' prefab) at your current spot.
 * 4. Press 2: Instantly teleports you back to that marker and destroys it.
 */

public class ElectricMagic : MonoBehaviour
{
    private PlayerInput _input;
    public Rigidbody2D agnes;
    private bool markerPlaced = false;
    private Vector3 markerPosition;
    private Movement plymov;
    public GameObject eletrik;
    private GameObject clone;

    void Start()
    {
        plymov = GetComponent<Movement>();
        _input = GetComponent<PlayerInput>();
    }

    void Update()
    {
        if (_input.actions["Electric"].WasPressedThisFrame())
        {
            if (!markerPlaced)
            {
                markerPosition = agnes.transform.position;
                markerPlaced = true;
                clone = Instantiate(eletrik, transform.position, transform.rotation);
            }
            else
            {
                agnes.transform.position = markerPosition;
                Destroy(clone);
                plymov.isHiding = false;
                agnes.constraints = RigidbodyConstraints2D.FreezeRotation;
                plymov.SetFrameVelocity(Vector2.zero);
                markerPlaced = false;
            }
        }
    }
}
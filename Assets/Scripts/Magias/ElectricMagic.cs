using UnityEngine;
using UnityEngine.InputSystem;

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
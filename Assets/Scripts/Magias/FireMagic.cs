using UnityEngine;
using UnityEngine.InputSystem;

public class FireMagic : MonoBehaviour
{
    [SerializeField] private ScriptableStats _stats;
    private Movement plymov;
    private PlayerInput _input;
    public float CannonSpeed;
    public float CannonAcceleration;

    void Start()
    {
        plymov = GetComponent<Movement>();
        _input = GetComponent<PlayerInput>();
    }

    void Update()
    {
        if (!plymov.isGrounded() && _input.actions["Fire"].IsPressed())
        {
            _stats.MaxFallSpeed = CannonSpeed;
            _stats.FallAcceleration = CannonAcceleration;
            plymov.usingFireMagic = true;
            plymov.usingWindMagic = false;
        }
        else if (!_input.actions["Fire"].IsPressed() || plymov.isGrounded())
        {
            _stats.MaxFallSpeed = 40;
            _stats.FallAcceleration = 80;
            plymov.usingFireMagic = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Destroyable") && plymov.usingFireMagic)
        {
            Destroy(collision.gameObject);
        }
    }
}
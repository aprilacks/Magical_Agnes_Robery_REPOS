using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

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
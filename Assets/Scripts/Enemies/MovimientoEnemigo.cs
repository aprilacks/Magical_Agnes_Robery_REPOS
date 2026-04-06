using UnityEngine;
/* * HOW TO USE:
 * 1. Attach to an Enemy GameObject.
 * 2. TOGGLE 'Is Camera Mode':
 * - OFF: Assign Point A and Point B (Transforms) for the enemy to patrol between.
 * - ON: Set Start/End angles for a stationary rotating security camera effect.
 * 3. Works in tandem with 'EnemyScript' to update the FOV direction based on movement.
 */
public class MovimientoEnemigo : MonoBehaviour
{
    #region Public Variables
    [Header("Mode Toggle")]
    public bool isCameraMode = false;

    [Header("Movement Settings (Patrol)")]
    public float speed = 2f;
    public float waitTime = 1.0f;
    public Transform pointA;
    public Transform pointB;

    [Header("Camera Mode Settings")]
    public float startAngle = 45f;
    public float endAngle = 135f;
    public float rotationSpeed = 0.5f;  
    public float pauseTime = 1.5f;     
    public bool clockwise = true;       
    #endregion

    #region Private Variables
    private Transform currentTarget;
    private SpriteRenderer spriteRenderer;
    private EnemyScript enemyScript;

    private float waitTimer;
    private bool isWaiting = false;

    // Camera Mode Tracking
    private float lerpFactor = 0f;
    private bool movingForward = true;
    #endregion

    public void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyScript = GetComponent<EnemyScript>();

        if (!isCameraMode)
        {
            currentTarget = pointB;
            UpdateFacing();
        }
        else if (enemyScript != null)
        {
            // Set initial rotation
            enemyScript.fovRotation = startAngle;
        }
    }

    void Update()
    {
        if (isCameraMode)
        {
            HandleCameraRotation();
        }
        else
        {
            HandlePatrol();
        }
    }

    void HandleCameraRotation()
    {
        if (enemyScript == null) return;

        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0) isWaiting = false;
            return;
        }

        // Increment or Decrement lerpFactor based on direction
        float step = rotationSpeed * Time.deltaTime;
        lerpFactor += movingForward ? step : -step;

        // Calculate the current rotation
        // If clockwise is false, we technically just swap the start/end behavior
        float targetRotation = Mathf.LerpAngle(startAngle, endAngle, lerpFactor);
        enemyScript.fovRotation = targetRotation;

        // Check if we hit the boundaries
        if (lerpFactor >= 1f || lerpFactor <= 0f)
        {
            lerpFactor = Mathf.Clamp01(lerpFactor);
            movingForward = !movingForward;
            isWaiting = true;
            waitTimer = pauseTime;
        }
    }

    void HandlePatrol()
    {
        if (currentTarget == null) return;

        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0)
            {
                isWaiting = false;
                currentTarget = (currentTarget == pointB) ? pointA : pointB;
                UpdateFacing();
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, currentTarget.position) < 0.1f)
            {
                isWaiting = true;
                waitTimer = waitTime;
            }
        }
    }

    public void UpdateFacing()
    {
        bool goingRight = currentTarget.position.x > transform.position.x;
        if (spriteRenderer != null) spriteRenderer.flipX = !goingRight;

        if (enemyScript != null)
        {
            enemyScript.fovRotation = goingRight ? 90f : 270f;
        }
    }
}
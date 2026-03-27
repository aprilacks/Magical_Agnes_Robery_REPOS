/* * HOW TO USE:
 * 1. Create two Empty GameObjects as children of your Enemy (PointA and PointB).
 * 2. Drag them into the 'Point A' and 'Point B' slots in the Inspector.
 * 3. Set 'Speed' for movement and 'Wait Time' for the pause duration at each point.
 */

using UnityEngine;

public class MovimientoEnemigo : MonoBehaviour
{
    #region Public Variables
    public float speed;
    public float waitTime = 1.0f; // Seconds to wait at each point
    public Transform pointA;
    public Transform pointB;
    #endregion

    #region Private Variables
    private Transform currentTarget;
    private SpriteRenderer spriteRenderer;
    private EnemyScript enemyScript;
    private float waitTimer;
    private bool isWaiting = false;
    #endregion

    public void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyScript = GetComponent<EnemyScript>();

        // Start by moving towards Point B
        currentTarget = pointB;
        UpdateFacing();
    }

    void Update()
    {
        if (currentTarget == null) return;

        if (isWaiting)
        {
            HandleWait();
        }
        else
        {
            Move();
        }
    }

    void Move()
    {
        // Move towards the current target
        transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);

        // Check if we have reached the target
        if (Vector2.Distance(transform.position, currentTarget.position) < 0.1f)
        {
            isWaiting = true;
            waitTimer = waitTime;
            // Note: We don't flip yet; we flip AFTER the wait is over.
        }
    }

    void HandleWait()
    {
        waitTimer -= Time.deltaTime;

        if (waitTimer <= 0)
        {
            isWaiting = false;

            // Switch targets
            currentTarget = (currentTarget == pointB) ? pointA : pointB;

            // Flip the sprite and view cone now that we are moving again
            UpdateFacing();
        }
    }

    public void UpdateFacing()
    {
        // Determine direction based on target position relative to enemy
        bool goingLeft = currentTarget.position.x < transform.position.x;

        spriteRenderer.flipX = goingLeft;

        if (enemyScript != null)
        {
            // If flipping X, point cone left (270), else point right (90)
            enemyScript.fovRotation = goingLeft ? 270f : 90f;
        }
    }

    // Visualizes the patrol path in the Scene view
    private void OnDrawGizmos()
    {
        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(pointA.position, pointB.position);
            Gizmos.DrawSphere(pointA.position, 0.1f);
            Gizmos.DrawSphere(pointB.position, 0.1f);
        }
    }
}
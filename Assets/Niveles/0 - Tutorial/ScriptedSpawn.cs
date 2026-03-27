/* * HOW TO USE:
 * 1. Place this on a GameObject with a Trigger Collider.
 * 2. Assign your Enemy Prefab to the 'Enemy' slot.
 * 3. 'Spawn Distance' determines how far the enemy travels before turning back (or being destroyed).
 */

using UnityEngine;

public class ScriptedSpawn : MonoBehaviour
{
    public GameObject Enemy;
    public float enemySpeed = 5f;
    public float spawnDistance = 10f; // How far they should run
    public float despawnTime = 8f;

    private GameObject clone;
    private bool guardSpawned = true;
    private MovimientoEnemigo movEnemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Only trigger if it's the player and we haven't spawned yet
        if (guardSpawned && collision.CompareTag("Player"))
        {
            guardSpawned = false;
            SpawnGuard();
        }
    }

    void SpawnGuard()
    {
        // 1. Instantiate the enemy
        clone = Instantiate(Enemy, transform.position, transform.rotation);
        movEnemy = clone.GetComponent<MovimientoEnemigo>();

        if (movEnemy != null)
        {
            // 2. Create the "Rail" points dynamically
            // We create two new Empty GameObjects to act as the patrol targets
            GameObject pA = new GameObject("Scripted_PointA");
            GameObject pB = new GameObject("Scripted_PointB");

            // Position Point A at the spawn location
            pA.transform.position = transform.position;
            // Position Point B 'spawnDistance' away to the left (matching your -500f logic but scaled)
            pB.transform.position = new Vector3(transform.position.x - spawnDistance, transform.position.y, transform.position.z);

            // 3. Assign them to the MovimientoEnemigo script
            movEnemy.pointA = pA.transform;
            movEnemy.pointB = pB.transform;
            movEnemy.speed = enemySpeed;

            // 4. Ensure the points are destroyed when the enemy is destroyed to avoid clutter
            pA.transform.SetParent(clone.transform);
            pB.transform.SetParent(clone.transform);
        }

        // 5. Cleanup
        Destroy(clone, despawnTime);
    }
}
using UnityEngine;

public class ScriptedSpawn : MonoBehaviour
{
    public GameObject Enemy;
    private GameObject clone;
    private float timer;
    private bool guardSpawned = true;
    private MovimientoEnemigo movEnemy;
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (guardSpawned)
        {
            guardSpawned = false;
            clone = Instantiate(Enemy, this.transform.position, this.transform.rotation);
            movEnemy = clone.GetComponent<MovimientoEnemigo>();
            movEnemy.speed = 5;
            movEnemy.leftLimit = transform.position .x - 500f;
            movEnemy.rightLimit = transform.position.x;
            Destroy(clone, 8f);
        }
    }
}

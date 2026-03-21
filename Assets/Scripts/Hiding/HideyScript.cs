using System.Runtime.CompilerServices;
using UnityEngine;

public class HideyScript : MonoBehaviour
{
    [SerializeField] private Movement _move;
    public Rigidbody2D agnes;
    private bool playerInRange = false;
    public Rigidbody2D hidespot;
    private CapsuleCollider2D disableCollision;

    private void Start()
    {
        disableCollision = agnes.GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.H))
        {
            if (!_move.isHiding)
            {
                Hide();
            }
            else
            {
                Unhide();
            }
        }
        if (_move.isHiding == true && playerInRange)
        {
            agnes.transform.position = hidespot.transform.position;
            agnes.constraints = RigidbodyConstraints2D.FreezePosition;
        }
    }

    private void Hide()
    {
        _move.isHiding = true;
        disableCollision.isTrigger = true;

    }

    private void Unhide()
    {
        _move.isHiding = false;
        disableCollision.isTrigger = false;
        agnes.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}


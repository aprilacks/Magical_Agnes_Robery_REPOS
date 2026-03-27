/* * HOW TO USE:
 * 1. Attached to the root of your Room Prefab.
 * 2. Ensure the child Virtual Camera is assigned in the Inspector.
 * 3. The LevelManager calls ActivateRoom() when this prefab is spawned.
 */

using UnityEngine;
using Unity.Cinemachine;

[RequireComponent(typeof(PolygonCollider2D))]
public class RoomController : MonoBehaviour
{
    [Header("Cinemachine Settings")]
    public CinemachineCamera vCam;
    [SerializeField] private int activePriority = 20;

    private void Start()
    {
        // When a room is instantiated, make sure the camera follows the player
        if (vCam != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                vCam.Follow = player.transform;
            }
        }
    }

    // Called by LevelManager immediately after Instantiate
    public void ActivateRoom()
    {
        if (vCam != null)
        {
            vCam.Priority = activePriority;
        }
    }

    // Safety check: if the player somehow exits the room trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ActivateRoom();
        }
    }
}
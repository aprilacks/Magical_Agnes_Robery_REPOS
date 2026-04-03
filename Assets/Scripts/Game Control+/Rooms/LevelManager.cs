/* * HOW TO USE:
 * 1. Create an empty GameObject named "LevelManager" in your Scene.
 * 2. Attach this script.
 * 3. Drag your Room Prefabs into the 'Room Prefabs' array in the inspector.
 * 4. Ensure each Room Prefab has a child object named "EntranceSpawnPoint".
 * 5. This manager will reset every time a new Scene is loaded.
 */

using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Room List")]
    [Tooltip("The order of room prefabs for THIS specific scene.")]
    public GameObject[] roomPrefabs;

    private GameObject currentRoomInstance;
    private int currentRoomIndex = 0;

    private void Awake()
    {
        // Simple instance reference for the current scene
        Instance = this;
    }

    private void Start()
    {
        if (roomPrefabs.Length > 0)
        {
            LoadRoom(0);
        }
        else
        {
            Debug.LogError("No room prefabs assigned to LevelManager in this scene!");
        }
    }

    public void LoadNextRoom()
    {
        currentRoomIndex++;
        if (currentRoomIndex < roomPrefabs.Length)
        {
            LoadRoom(currentRoomIndex);
        }
        else
        {
            Debug.Log("End of stages in this scene reached!");
        }
    }

    public void ResetCurrentRoom()
    {
        LoadRoom(currentRoomIndex);
    }

    private void LoadRoom(int index)
    {
        // 1. Cleanup old room
        if (currentRoomInstance != null)
        {
            Destroy(currentRoomInstance);
        }

        // 2. Spawn new room
        currentRoomInstance = Instantiate(roomPrefabs[index], Vector3.zero, Quaternion.identity);

        // 3. Teleport Player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // Reset velocity to prevent carrying momentum between stages
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null) rb.linearVelocity = Vector2.zero;

            Transform spawnPoint = currentRoomInstance.transform.Find("EntranceSpawnPoint");
            if (spawnPoint != null)
            {
                player.transform.position = spawnPoint.position;
            }
        }

        // 4. Update Camera through RoomController
        RoomController rc = currentRoomInstance.GetComponent<RoomController>();
        if (rc != null)
        {
            rc.ActivateRoom();
        }
    }
}
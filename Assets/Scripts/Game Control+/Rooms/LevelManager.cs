using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Room List")]
    public GameObject[] roomPrefabs;

    private GameObject currentRoomInstance;
    private int currentRoomIndex = 0;

    private void Awake()
    {
        // Singleton pattern to ensure only one LevelManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (roomPrefabs.Length > 0)
        {
            LoadRoom(0);
        }
        else
        {
            Debug.LogError("No room prefabs assigned to LevelManager!");
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
            Debug.Log("End of Game reached!");
        }
    }

    public void ResetCurrentRoom()
    {
        Debug.Log("Resetting Room Index: " + currentRoomIndex);
        LoadRoom(currentRoomIndex);
    }

    private void LoadRoom(int index)
    {
        // Cleans up the old room
        if (currentRoomInstance != null)
        {
            Destroy(currentRoomInstance);
        }

        // Spawns the new room prefab at coordinates
        currentRoomInstance = Instantiate(roomPrefabs[index], Vector3.zero, Quaternion.identity);

        // Teleports the player to the SpawnPoint inside the NEW room
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // Resets player physics so they don't carry momentum into the new room
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null) rb.linearVelocity = Vector2.zero;

            // Finds the spawn point object inside the instantiated room
            Transform spawnPoint = currentRoomInstance.transform.Find("EntranceSpawnPoint");
            if (spawnPoint != null)
            {
                player.transform.position = spawnPoint.position;
            }
            else
            {
                Debug.LogWarning("EntranceSpawnPoint not found in " + roomPrefabs[index].name);
            }
        }

        // 4. Update the Camera
        RoomController rc = currentRoomInstance.GetComponent<RoomController>();
        if (rc != null)
        {
            rc.ActivateRoom();
        }
    }
}
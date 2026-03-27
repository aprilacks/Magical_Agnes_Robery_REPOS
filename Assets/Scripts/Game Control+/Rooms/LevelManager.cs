/* * HOW TO USE:
 * 1. Create an Empty GameObject named "LevelManager".
 * 2. Drag all your Room Prefabs into the 'roomPrefabs' list in order.
 * 3. This script handles spawning the next room and deleting the old one.
 */

using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public GameObject[] roomPrefabs;
    private GameObject currentRoomInstance;
    private int currentRoomIndex = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Load the very first room at the start of the game
        LoadRoom(0);
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
            Debug.Log("End of Game! No more rooms.");
        }
    }

    void LoadRoom(int index)
    {
        // 1. Delete the old room if it exists
        if (currentRoomInstance != null)
        {
            Destroy(currentRoomInstance);
        }

        // 2. Spawn the new room at (0,0,0) 
        // We keep rooms at the same world space; the camera handles the rest.
        currentRoomInstance = Instantiate(roomPrefabs[index], Vector3.zero, Quaternion.identity);

        // 3. Move player to the entrance of the new room
        // We look for the "SpawnPoint" tag or child in the newly spawned room
        Transform spawnPoint = currentRoomInstance.transform.Find("EntranceSpawnPoint");
        if (spawnPoint != null)
        {
            GameObject.FindGameObjectWithTag("Player").transform.position = spawnPoint.position;
        }

        // 4. Update Cinemachine for the new room
        RoomController rc = currentRoomInstance.GetComponent<RoomController>();
        if (rc != null) rc.ActivateRoom();
    }
}
using UnityEngine;

public class CameraReference : MonoBehaviour
{
    [Tooltip("Camera Script Object for Reference")]
    public Camera_Controller_Structure cameraRef = null;

    [Header("Camera")]
    public GameObject cam;

    private void OnTriggerEnter2D(Collider2D collided)
    {
        cam = collided.gameObject;
       


        // If collition is detected and This object has a Camera Reference Game Object
        if (collided != null && cameraRef != null)
        {
            // If Game Object Tag is equal to the Bg Tag
            if (collided.gameObject.CompareTag("ChangeCamera") || collided.gameObject.CompareTag("VerticalScroll") || collided.gameObject.CompareTag("HorizontalScroll"))
            {
                // COLIDED GAME OBJECT
                cameraRef.ColliderTarget = collided.gameObject;
                // COLIDED GAME OBJECT POSITION AND SCALE
                Vector3 pos = collided.transform.position;
                Vector3 scale = collided.transform.localScale;
                
                // BOUNDARIES OF COLLIDED GAME OBJECT
                float minX = pos.x - scale.x/2;
                float maxX = pos.x + scale.x/2;
                float minY = pos.y - scale.y/2;
                float maxY = pos.y + scale.y/2;
                cameraRef.minX = minX;
                cameraRef.maxX = maxX;
                cameraRef.minY = minY;
                cameraRef.maxY = maxY;

                // SEE LOGIC IN CAMERA_CONTROLLER
            }

        }
    }


}

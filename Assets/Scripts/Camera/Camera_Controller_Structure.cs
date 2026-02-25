using Unity.VisualScripting;
using UnityEngine;

public class Camera_Controller_Structure : MonoBehaviour
{
    [Header("Camera Movement Structure")]
    
    [Tooltip("Target Game Object, It becomes a Collider [[ See CameraReference ]]")]
    public GameObject ColliderTarget = null;

    [Tooltip("Target Game Object is the Player")]
    public GameObject Player;
    [Header("Canera Object")]
    public Camera CAMERA;


    [Tooltip("Lerp Transition Speed")]
    public float speed = 1.0f;

    [Tooltip("Sets an Offset in Camera's Position if needed")]
    public Vector3 offset = Vector3.zero;
    public float scaleOffset;
    public Vector3 scaleOffsetVector;

    [Tooltip("Camera's initial Position")]
    public Vector3 baseCamPosition = Vector3.zero;
    Vector3 FinalTarget;

    [Header("Movemente Script for _grounded")]
    public Movement _movement;

    public bool prueba;

    void Start()
    {
        FinalTarget = Vector3.zero;


        // If target has an asigned Game object
        if (ColliderTarget != null)
        { 
            baseCamPosition = ColliderTarget.transform.position;
            transform.position = baseCamPosition;
        }// Camera Initial Position = Target position
    }


    void Update()
    {
        // If target has an asigned Game object
        if (ColliderTarget != null || Player != null)
        {
            if (ColliderTarget.gameObject == null) return; 

            if (ColliderTarget.gameObject.tag == "ChangeCamera")
            {
                offset.x = 0;
                offset.y = 0;
                FinalTarget = ColliderTarget.transform.position;
                // CAMERA MOVES TOWARDS OBJECTIVE
               
                baseCamPosition = Vector3.Lerp(baseCamPosition, FinalTarget + offset, speed * Time.deltaTime);
               
            }
            else if(ColliderTarget.gameObject.tag == "VerticalScroll")
            {
                offset.x = 0;
                offset.y = 5;

                FinalTarget = new Vector3 (ColliderTarget.transform.position.x, Player.transform.position.y);

                if(!_movement.isGrounded() || _movement.isFalling())
                {
                    offset.y = -2;
                    // CAMERA MOVES TOWARDS OBJECTIVE
                    baseCamPosition = Vector3.Lerp(baseCamPosition, FinalTarget + offset, speed * Time.deltaTime);
                }
            }
            else if (ColliderTarget.gameObject.tag == "HorizontalScroll")
            {
                offset.y = 0;
                offset.x = 5 * _movement.ReturnDirection();

                if (ColliderTarget.transform.localScale.x + ColliderTarget.transform.localScale.x / 2 - CAMERA.orthographicSize >= CAMERA.orthographicSize)
                {
                    prueba = false;
                }

                scaleOffset = ColliderTarget.transform.localScale.x + ColliderTarget.transform.localScale.x / 2 - CAMERA.orthographicSize;
                scaleOffsetVector = ColliderTarget.transform.localScale /2;  //.x + ColliderTarget.transform.localScale.x/2 - CAMERA.orthographicSize;

                FinalTarget = new Vector3(Player.transform.position.x, ColliderTarget.transform.position.y);
                // CAMERA MOVES TOWARDS OBJECTIVE
                baseCamPosition = Vector3.Lerp(baseCamPosition, FinalTarget + offset, speed * Time.deltaTime);


            }

            /// transform.scale.x + transform.scale.x/2 - Tamaño de la Cámara en x (HACERLO EN TODOS LOS LADOS)


            // CAMERA LOCKS INTO THE OBJECTIVE
            // Makes a transition to the next Collided Object tagged by ChangeCamera [[ See CameraReference ]]
            transform.position = baseCamPosition;

        }



    }
}

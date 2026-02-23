using System.Collections;
using UnityEngine;

public class WaterMagic : MonoBehaviour
{
    [SerializeField] private ScriptableStats _stats;
    private Movement plymov = null;
    public Rigidbody2D agnes;
    private bool DashUsed = false;

    public Sprite water;
    public Sprite notWater;

    public Vector2 WaterDashForce;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        plymov = GetComponent<Movement>();
    }


    // Update is called once per frame
    void Update()
    {
        WaterDash();
        if (plymov.isGrounded()) DashUsed = false;
    }

    void WaterDash()
    {
        if (Input.GetKeyDown(KeyCode.V) && DashUsed == false)
        {
            GetComponent<SpriteRenderer>().sprite = water;
            WaterDashForce.x *= plymov.ReturnDirection();
            agnes.constraints = RigidbodyConstraints2D.FreezePositionY;
            plymov.SetFrameVelocity(WaterDashForce);
            DashUsed = true;
            StartCoroutine(WaitForSeconds());
        }
        else if(!Input.GetKeyDown(KeyCode.V) && plymov.usingWindMagic == false)
        {
            
            plymov.usingWaterMagic = false;
            GetComponent<SpriteRenderer>().sprite = notWater;
        }
    }

    IEnumerator WaitForSeconds()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        plymov.SetFrameVelocity(-WaterDashForce/3);
        WaterDashForce.x = 40;
        agnes.constraints = RigidbodyConstraints2D.None;
    }

}

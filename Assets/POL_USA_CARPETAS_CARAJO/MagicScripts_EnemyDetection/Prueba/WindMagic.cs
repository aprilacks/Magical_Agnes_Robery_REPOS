using UnityEngine;

public class WindMagic : MonoBehaviour
{
    [SerializeField] private ScriptableStats _stats;
    public Rigidbody2D agnes;
    private Movement plymov = null;
    public float fallspeed;
    public float slowmo;
    public Sprite wind;
    public Sprite notWind;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        plymov = GetComponent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (plymov == null) return;
        if (Input.GetKey(KeyCode.Z))
        {
             _stats.MaxFallSpeed = fallspeed;
              plymov.usingWindMagic = true;
             _stats.MaxSpeed = slowmo;
            GetComponent<SpriteRenderer>().sprite = wind;
        }
        else
        {
            _stats.MaxFallSpeed = 40;
            plymov.usingWindMagic = false;
            _stats.MaxSpeed = 14;
            GetComponent<SpriteRenderer>().sprite = notWind;
        }
    }
}

using UnityEngine;

public class AnimationController : Movement
{
    Animator _anim;
    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _anim.SetBool("grounded", _grounded);
        _anim.SetBool("Water", usingWaterMagic);
        _anim.SetBool("Wind", usingWindMagic);
        _anim.SetBool("Fire", usingFireMagic);
        if((_rb.linearVelocity.x > 2f || _rb.linearVelocity.x < -2f) && _grounded)
        {
            _anim.SetBool("Moving", true);
        }
        else
        {
            _anim.SetBool("Moving", false);
        }
        if (_rb.linearVelocity.y > 1f && !_grounded)
        {
            _anim.SetBool("Jumping", true);
        }
        else if (_rb.linearVelocity.y < 1f && !_grounded)
        {
            _anim.SetBool("Falling", true);
            _anim.SetBool("Jumping", false);
        }
        else
        {
            _anim.SetBool("Falling", false);
        }

    }
}

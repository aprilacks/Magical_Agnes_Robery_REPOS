using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D), typeof(PlayerInput))]
[RequireComponent(typeof(AudioSource))]
public class Movement : MonoBehaviour, IPlayerController
{
    [SerializeField] private ScriptableStats _stats;

    [Header("Master Audio Clips")]
    [SerializeField] private AudioClip _walkClip;
    [SerializeField] private AudioClip _windGlideClip;
    [SerializeField] private AudioClip _fireCannonballClip;
    [SerializeField] private AudioClip _waterDashClip;
    [SerializeField][Range(0, 1)] private float _masterVolume = 0.5f;

    private PlayerInput _input;
    private AudioSource _audioSource;
    [HideInInspector] public Rigidbody2D _rb;
    private CapsuleCollider2D _col;
    private FrameInput _frameInput;
    private Vector2 _frameVelocity;
    private bool _cachedQueryStartInColliders;
    private Quaternion noRotate = Quaternion.identity;

    [Header("Magic States")]
    public bool usingFireMagic = false;
    public bool usingWindMagic = false;
    public bool usingWaterMagic = false;
    public bool isHiding = false;

    [Header("Grounding")]
    public bool _grounded;
    private float _lastGroundedTime;
    [SerializeField] private float _groundedGracePeriod = 0.05f;

    public Vector2 castSize;


    public Vector2 FrameInput => _frameInput.Move;
    public event Action<bool, float> GroundedChanged;
    public event Action Jumped;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<CapsuleCollider2D>();
        _input = GetComponent<PlayerInput>();
        _audioSource = GetComponent<AudioSource>();

        _audioSource.playOnAwake = false;
        _audioSource.volume = _masterVolume;
        _cachedQueryStartInColliders = Physics2D.queriesStartInColliders;
    }

    private void Update()
    {
        GatherInput();
        HandleMasterAudio();
        transform.rotation = noRotate;
    }

    public void PlayDashSound()
    {
        if (_waterDashClip != null)
        {
            // We use PlayOneShot so it doesn't overwrite the 'clip' variable used for loops
            _audioSource.PlayOneShot(_waterDashClip, _masterVolume);
        }
    }

    private void HandleMasterAudio()
    {
        AudioClip desiredLoop = null;

        // 1. Determine which LOOPING sound should be active
        if (usingWindMagic)
        {
            desiredLoop = _windGlideClip;
        }
        else if (usingFireMagic)
        {
            desiredLoop = _fireCannonballClip;
        }
        else if (_grounded && Mathf.Abs(_rb.linearVelocity.x) > 0.5f && !isHiding)
        {
            desiredLoop = _walkClip;
        }

        // 2. Handle Looping Logic
        if (desiredLoop != null)
        {
            // If we are switching loops or starting a new one
            if (_audioSource.clip != desiredLoop || !_audioSource.isPlaying)
            {
                _audioSource.clip = desiredLoop;
                _audioSource.loop = true;
                _audioSource.Play();
            }
        }
        else
        {
            // IMPORTANT FIX: 
            // Only stop if we are currently playing a LOOP. 
            // If a OneShot (Dash) is playing, clip stays null or old, but isPlaying is true.
            // We check if the current playing sound is marked as a loop.
            if (_audioSource.isPlaying && _audioSource.loop)
            {
                _audioSource.Stop();
                _audioSource.loop = false; // Reset loop flag
            }
        }
    }

    private void GatherInput()
    {
        _frameInput = new FrameInput
        {
            JumpDown = _input.actions["Jump"].WasPressedThisFrame(),
            Move = _input.actions["Move"].ReadValue<Vector2>()
        };
        if (_frameInput.JumpDown) _jumpToConsume = true;
    }

    private void FixedUpdate()
    {
        if (isHiding) { _rb.linearVelocity = Vector2.zero; return; }
        CheckCollisions();
        HandleJump();
        HandleDirection();
        HandleGravity();
        _rb.linearVelocity = _frameVelocity;
    }

    private void CheckCollisions()
    {
        Physics2D.queriesStartInColliders = false;
        castSize = new Vector2(_col.size.x * 0.9f, _col.size.y);
        bool groundHit = Physics2D.CapsuleCast(_col.bounds.center, castSize, _col.direction, 0, Vector2.down, _stats.GrounderDistance, ~_stats.PlayerLayer);

        if (groundHit)
        {
            if (!_grounded)
            {
                _grounded = true;
                GroundedChanged?.Invoke(true, Mathf.Abs(_rb.linearVelocity.y));
            }
            _lastGroundedTime = Time.time;
        }
        else if (_grounded && Time.time > _lastGroundedTime + _groundedGracePeriod)
        {
            _grounded = false;
            GroundedChanged?.Invoke(false, 0);
        }
        Physics2D.queriesStartInColliders = _cachedQueryStartInColliders;
    }
    // DRAW GROUND CHECK
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = _grounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(_col.bounds.center, _stats.GrounderDistance);
    }

    private bool _jumpToConsume;
    private void HandleJump()
    {
        if (_jumpToConsume && _grounded)
        {
            _frameVelocity.y = _stats.JumpPower;
            _jumpToConsume = false;
            Jumped?.Invoke();
        }
    }

    private void HandleDirection()
    {
        if (_frameInput.Move.x == 0)
        {
            var decel = _grounded ? _stats.GroundDeceleration : _stats.AirDeceleration;
            _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, 0, decel * Time.fixedDeltaTime);
        }
        else
        {
            _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, _frameInput.Move.x * _stats.MaxSpeed, _stats.Acceleration * Time.fixedDeltaTime);
        }
    }

    private void HandleGravity()
    {
        if (_grounded && _frameVelocity.y < 0) _frameVelocity.y = 0f;
        else _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, -_stats.MaxFallSpeed, _stats.FallAcceleration * Time.fixedDeltaTime);
    }

    public void SetFrameVelocity(Vector2 velocity) => _frameVelocity = velocity;
    public float ReturnDirection() => Mathf.Sign(transform.localScale.x);
    public bool isGrounded() => _grounded;
}

public struct FrameInput { public bool JumpDown; public Vector2 Move; }
public interface IPlayerController { public event Action<bool, float> GroundedChanged; public event Action Jumped; public Vector2 FrameInput { get; } }
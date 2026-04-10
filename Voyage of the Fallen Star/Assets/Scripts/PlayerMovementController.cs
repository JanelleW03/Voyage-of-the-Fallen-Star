using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    private static readonly int MoveX = Animator.StringToHash("MoveX");
    private static readonly int MoveY = Animator.StringToHash("MoveY");
    public float speed;
    public float groundDist;
    
    public LayerMask terrainLayer;
    public SpriteRenderer spriteRenderer;
    public Transform attackPoints;
    
    private Rigidbody _body;
    private InputAction _moveAction;
    private Animator _animator;

    [Header("Audio")]
    [SerializeField]
    AudioSource walkingAudio;

    private void Start()
    {
        _body = GetComponent<Rigidbody>();
        _body.freezeRotation = true; 
        
        _moveAction = InputSystem.actions.FindAction("Move");
        
        _animator = GetComponentInChildren<Animator>();
    }



    private void FixedUpdate()
    {
        if (Time.timeScale == 0f)
        {
            _body.linearVelocity = Vector3.zero;
            return;
        }

        Vector2 moveValue = _moveAction.ReadValue<Vector2>();
        Vector3 moveDir = new Vector3(moveValue.x, 0, moveValue.y).normalized;

        // Preserve Y velocity so gravity works naturally
        Vector3 newVelocity = new Vector3(moveDir.x * speed, _body.linearVelocity.y, moveDir.z * speed);
        _body.linearVelocity = newVelocity;

        // Audio
        if (moveDir.magnitude > 0)
        {
            if (!walkingAudio.isPlaying)
                walkingAudio.Play();
        }
        else
        {
            if (walkingAudio.isPlaying)
                walkingAudio.Stop();
        }

        // Animator + sprite flipping
        _animator.SetFloat(MoveX, moveValue.x);
        _animator.SetFloat(MoveY, moveValue.y);
        if (moveValue.x != 0)
        {
            bool isMovingLeft = moveValue.x < 0;
            spriteRenderer.flipX = isMovingLeft;
            if (attackPoints)
                attackPoints.localRotation = Quaternion.Euler(0, isMovingLeft ? 180f : 0f, 0);
        }
        else if (moveValue.y != 0)
        {
            bool isMovingBack = moveValue.y < 0;
            spriteRenderer.flipX = isMovingBack;
            if (attackPoints)
                attackPoints.localRotation = Quaternion.Euler(0, isMovingBack ? 90f : -90f, 0);
        }
    }




    public void SetMovementEnabled(bool enabled)
    {
        if (enabled)
            _moveAction.Enable();
        else
            _moveAction.Disable();
    }
}
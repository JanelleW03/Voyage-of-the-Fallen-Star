using UnityEngine;
using UnityEngine.InputSystem;

// TODO: When the player sprite is updated, don't forget to also update the collider!
public class PlayerMovementController : MonoBehaviour
{
    public float speed;
    public float groundDist;
    
    public LayerMask terrainLayer;
    public SpriteRenderer spriteRenderer;
    public Transform attackPoints;
    
    private Rigidbody _body;
    private InputAction _moveAction;

    [Header("Audio")]
    [SerializeField]
    AudioSource walkingAudio;

    private void Start()
    {
        _body = GetComponent<Rigidbody>();
        _body.freezeRotation = true; 
        
        _moveAction = InputSystem.actions.FindAction("Move");
    }

    private void FixedUpdate()
    {
        if (Time.timeScale == 0f)
        {
            _body.linearVelocity = Vector3.zero;
            return;
        }

        Vector3 castPos = transform.position + Vector3.up;
        if (Physics.Raycast(castPos, Vector3.down, out RaycastHit hit, Mathf.Infinity, terrainLayer))
        {
            Vector3 movePos = transform.position;
            movePos.y = hit.point.y + groundDist;
            transform.position = movePos;
        }
        
        Vector2 moveValue = _moveAction.ReadValue<Vector2>();
        Vector3 moveDir = new Vector3(moveValue.x, 0, moveValue.y).normalized;
        
        _body.linearVelocity = new Vector3(moveDir.x * speed, 0, moveDir.z * speed);

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


        if (moveValue.x != 0)
        {
            bool isMovingLeft = moveValue.x < 0;
            spriteRenderer.flipX = isMovingLeft;

            if (attackPoints)
            {
                attackPoints.localRotation = Quaternion.Euler(0, isMovingLeft ? 180f : 0f, 0);
            }
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
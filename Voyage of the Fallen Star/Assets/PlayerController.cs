using UnityEngine;
using UnityEngine.Events;

// TODO: When the player sprite is updated, don't forget to also update the collider!
public class PlayerController : MonoBehaviour
{
    public float speed;
    public float groundDist;
    
    public LayerMask terrainLayer;
    public SpriteRenderer spriteRenderer;
    
    private Rigidbody _body;

    // for the dialogue

    private void Start()
    {
        _body = gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Vector3 castPos = transform.position;
        castPos.y += 1;
        if (Physics.Raycast(castPos, -transform.up, out RaycastHit hit, Mathf.Infinity, terrainLayer))
        {
            if (hit.collider)
            {
                Vector3 movePos = transform.position;
                movePos.y = hit.point.y + groundDist;
                transform.position = movePos;
            }
        }
        
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector3 moveDir = new Vector3(x, 0, y);
        _body.linearVelocity = moveDir * speed;

        spriteRenderer.flipX = x switch
        {
            < 0 => true,
            > 0 => false,
            _ => spriteRenderer.flipX
        };
    }
}

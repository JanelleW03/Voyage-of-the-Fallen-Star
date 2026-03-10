using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Transform player;
    
    public float chaseRange;
    public float attackRange;

    public float attackCooldown;
    public float damage;
    public bool isHostile; 

    private NavMeshAgent _agent;
    private float _lastAttackTime;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        
        _agent.updateRotation = false;
    }

    private void Update()
    {
        if (!isHostile || !player) 
        {
            _agent.ResetPath();
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            AttackPlayer();
        }
        else if (distanceToPlayer <= chaseRange)
        {
            ChasePlayer();
        }
        else
        {
            _agent.ResetPath();
        }
        
        if (_agent.velocity.x != 0)
        {
            _spriteRenderer.flipX = _agent.velocity.x < 0;
        }
    }

    private void ChasePlayer()
    {
        _agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        _agent.ResetPath();
        
        if (Time.time >= _lastAttackTime + attackCooldown)
        {
            HealthComponent playerHealth = player.GetComponent<HealthComponent>();
            if (playerHealth)
            {
                playerHealth.TakeDamage(damage);
            }
            _lastAttackTime = Time.time;
        }
    }
}
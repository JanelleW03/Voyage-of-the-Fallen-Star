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

    protected NavMeshAgent _agent;
    protected float _lastAttackTime;
    private SpriteRenderer _spriteRenderer;

    protected virtual void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        _agent.updateRotation = false;
    }

    protected virtual void Update()
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

    protected virtual void AttackPlayer()
    {
        _agent.ResetPath();

        if (Time.time >= _lastAttackTime + attackCooldown)
        {
            Debug.Log("Attacking player!");
            HealthComponent playerHealth = player.GetComponentInChildren<HealthComponent>();
            if (playerHealth)
            {
                Debug.Log($"Found health component: {playerHealth.GetType().Name}, dealing {damage} damage");
                playerHealth.TakeDamage(damage);
            }
            _lastAttackTime = Time.time;
        }
    }
}
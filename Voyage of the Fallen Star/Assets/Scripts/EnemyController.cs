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
        _agent.updateUpAxis = false;
        _agent.autoBraking = false;
        _agent.enabled = true;


        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

    }

    protected virtual void Update()
    {
        if (!isHostile || !player)
        {
            //Debug.Log($"[{name}] Inactive — isHostile: {isHostile}, player: {(player ? player.name : "NULL")}");
            return;
        }

        if (!_agent.enabled)
        {
            _agent.enabled = true;
            _agent.updateRotation = false;
            _agent.updateUpAxis = false;
            _agent.autoBraking = false;
        }
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            Debug.Log($"[{name}] In ATTACK range (dist: {distanceToPlayer:F2})");
            _agent.isStopped = true;
            _agent.velocity = Vector3.zero;
            AttackPlayer();
        }
        else if (distanceToPlayer <= chaseRange)
        {
            Debug.Log($"[{name}] CHASING — dist: {distanceToPlayer:F2}, agent stopped: {_agent.isStopped}, velocity: {_agent.velocity}");

            _agent.isStopped = false;
            _agent.autoBraking = false;
            _agent.stoppingDistance = attackRange * 0.9f;
            _agent.SetDestination(player.position);
        }
        else
        {
            Debug.Log($"[{name}] Out of range — dist: {distanceToPlayer:F2}, chaseRange: {chaseRange}");
            _agent.isStopped = true;
            _agent.ResetPath();
            _agent.velocity = Vector3.zero;
        }

        if (_agent.velocity.x != 0)
            _spriteRenderer.flipX = _agent.velocity.x < 0;
    }
    protected virtual void AttackPlayer()
    {
        // Removed ResetPath() — let the agent hold position naturally via isStopped
        if (Time.time >= _lastAttackTime + attackCooldown)
        {
            HealthComponent playerHealth = player.GetComponentInChildren<HealthComponent>();
            if (playerHealth)
                playerHealth.TakeDamage(damage);

            _lastAttackTime = Time.time;
        }
    }


    public void SetHostile(bool hostile)
    {
        isHostile = hostile;
        if (hostile)
        {
            _agent.enabled = true;
            _agent.updateRotation = false;
            _agent.updateUpAxis = false;
            _agent.autoBraking = false;
        }
    }

}
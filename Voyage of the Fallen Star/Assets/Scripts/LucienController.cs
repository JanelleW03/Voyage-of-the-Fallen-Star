using System;
using UnityEngine;

public class LucienController : EnemyController
{
    private static readonly int MeleeAttackTrigger = Animator.StringToHash("MeleeAttackTrigger");
    private static readonly int MoveX = Animator.StringToHash("MoveX");
    private static readonly int MoveY = Animator.StringToHash("MoveY");
    
    private Animator _animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        _animator = GetComponentInChildren<Animator>();
    }

    private void FixedUpdate()
    {
        // Animator + sprite flipping
        Vector3 moveValue = _agent.desiredVelocity;
        _animator.SetFloat(MoveX, moveValue.x);
        _animator.SetFloat(MoveY, moveValue.y);
    }

    protected override void AttackPlayer()
    {
        _agent.ResetPath();

        if (Time.time >= _lastAttackTime + attackCooldown)
        {
            HealthComponent playerHealth = player.GetComponentInChildren<HealthComponent>();
            if (playerHealth)
            {
                _animator.SetTrigger(MeleeAttackTrigger);
                playerHealth.TakeDamage(damage);
            }
            _lastAttackTime = Time.time;
        }
    }
}

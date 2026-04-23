using UnityEngine;

public class LucienHealthComponent : NpcEnemyHealthComponent
{
    private static readonly int Evil = Animator.StringToHash("Evil");
    
    private Animator _animator;
    
    protected override void Start()
    {
        base.Start();
        _animator = GetComponentInChildren<Animator>();
    }

    protected override void Die()
    {
        base.Die();
        _animator.SetFloat(Evil, 1);
    }
}

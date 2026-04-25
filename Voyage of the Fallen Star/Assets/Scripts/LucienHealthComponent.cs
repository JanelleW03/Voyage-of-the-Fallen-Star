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

    protected override bool CountsAsNPC => false; // he's the boss, not an NPC kill

    protected override void Die()
    {
        base.Die(); // cleanup runs, but ReportNPCDefeated is skipped
        _animator.SetFloat(Evil, 1);
        VictoryManager.Instance?.ReportBossDefeated();
    }

}

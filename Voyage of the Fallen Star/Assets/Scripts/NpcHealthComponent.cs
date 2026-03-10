using UnityEngine;

public class NpcEnemyHealthComponent : HealthComponent
{
    public float manaReward = 25f;
    public SpriteRenderer spriteRenderer;
    
    protected override void Die()
    {
        StatsManager stats = FindFirstObjectByType<StatsManager>();
        if (stats)
        {
            stats.AddMana(manaReward);
        }

        EnemyController ai = GetComponent<EnemyController>();
        if (ai)
        {
            ai.isHostile = false;
        }

        healthSlider.gameObject.SetActive(false);
        spriteRenderer.color = Color.white;
    }
}

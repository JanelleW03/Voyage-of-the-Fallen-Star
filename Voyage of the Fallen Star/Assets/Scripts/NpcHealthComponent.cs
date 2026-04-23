using UnityEngine;

public class NpcEnemyHealthComponent : HealthComponent
{
    public float manaReward = 25f;
    public SpriteRenderer spriteRenderer;
    public Sprite passiveSprite;
    public Sprite hostileSprite; 

    public void StartCombat()
    {
        if (spriteRenderer != null && hostileSprite != null)
            spriteRenderer.sprite = hostileSprite;

        if (healthSlider != null)
            healthSlider.gameObject.SetActive(true);
    }

    protected override void Die()
    {
        StatsManager stats = FindFirstObjectByType<StatsManager>();
        if (stats)
            stats.AddMana(manaReward);

        EnemyController ai = GetComponent<EnemyController>();
        if (ai)
            ai.isHostile = false;

        if (healthSlider != null)
            healthSlider.gameObject.SetActive(false);

        if (spriteRenderer != null && passiveSprite != null)
            spriteRenderer.sprite = passiveSprite;
    }
}
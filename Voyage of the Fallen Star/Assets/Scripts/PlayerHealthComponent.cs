public class PlayerHealthComponent : HealthComponent
{
    protected override void Die()
    {
        GameOverScreen.Instance?.Show();
    }
}
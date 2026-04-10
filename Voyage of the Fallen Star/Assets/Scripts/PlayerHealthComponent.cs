using UnityEngine.SceneManagement;

public class PlayerHealthComponent: HealthComponent
{
    protected override void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
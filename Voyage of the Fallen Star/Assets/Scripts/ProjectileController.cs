using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float damage;
    public float lifetime;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) return;

        HealthComponent targetHealth = other.GetComponent<HealthComponent>();
        if (targetHealth)
        {
            targetHealth.TakeDamage(damage);
        }
        
        Destroy(gameObject);
    }
}
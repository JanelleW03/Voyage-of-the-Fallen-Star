using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    public float maxHealth = 100f;
    
    private float _currentHealth;

    public Slider healthSlider;
    
    private void Start()
    {
        _currentHealth = maxHealth;
        
        healthSlider.maxValue = maxHealth;
        healthSlider.value = _currentHealth;
    }

    public void TakeDamage(float amount)
    {
        if (_currentHealth <= 0) return;

        _currentHealth -= amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, maxHealth);
        
        UpdateUI();

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    // TODO: Potions
    public void Heal(float amount)
    {
        if (_currentHealth <= 0) return;

        _currentHealth += amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, maxHealth);
        
        UpdateUI();
    }

    private void UpdateUI()
    {
        healthSlider.value = _currentHealth;
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class StatsManager : MonoBehaviour
{ 
    public float maxMana = 100f;
    public Slider manaSlider;

    public float maxEnergy = 100f;
    public float energyRegenRate = 15f; 
    public Slider energySlider;
    
    private float _currentMana;
    private float _currentEnergy;

    private void Start()
    {
        _currentEnergy = maxEnergy;
        
        if (manaSlider) manaSlider.maxValue = maxMana;
        if (energySlider) energySlider.maxValue = maxEnergy;
        
        UpdateUI();
    }

    private void Update()
    {
        if (_currentEnergy < maxEnergy)
        {
            _currentEnergy += energyRegenRate * Time.deltaTime;
            _currentEnergy = Mathf.Min(_currentEnergy, maxEnergy);
            if (energySlider) energySlider.value = _currentEnergy;
        }
    }

    public void AddMana(float amount)
    {
        _currentMana += amount;
        _currentMana = Mathf.Clamp(_currentMana, 0, maxMana);
        if (manaSlider) manaSlider.value = _currentMana;
    }

    public bool TryConsumeMana()
    {
        if (_currentMana >= maxMana)
        {
            _currentMana = 0;
            UpdateUI();
            return true;
        }
        return false;
    }

    public bool TryConsumeEnergy(float amount)
    {
        if (_currentEnergy >= amount)
        {
            _currentEnergy -= amount;
            UpdateUI();
            return true;
        }
        return false;
    }

    private void UpdateUI()
    {
        if (manaSlider) manaSlider.value = _currentMana;
        if (energySlider) energySlider.value = _currentEnergy;
    }
}
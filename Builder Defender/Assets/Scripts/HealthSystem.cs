using System;
using UnityEngine;
using UnityEngine.Serialization;

public class HealthSystem : MonoBehaviour {

    public event EventHandler OnDamaged;
    public event EventHandler OnHealed;
    public event EventHandler OnDied;
    
    [SerializeField] private int healthAmountMax;
    private int _healthAmount;

    private void Awake() {
        _healthAmount = healthAmountMax;
    }

    public void Damage(int damageAmount) {
        _healthAmount -= damageAmount;
        _healthAmount = Mathf.Clamp(_healthAmount, 0, healthAmountMax);
        
        OnDamaged?.Invoke(this, EventArgs.Empty);

        if (IsDead()) {
            OnDied?.Invoke(this, EventArgs.Empty);
        }
    }
    
    public void Heal(int healAmount) {
        _healthAmount += healAmount;
        _healthAmount = Mathf.Clamp(_healthAmount, 0, healthAmountMax);
        
        OnHealed?.Invoke(this, EventArgs.Empty);
    }
    
    public void HealFull() {
        _healthAmount = healthAmountMax;
        
        OnHealed?.Invoke(this, EventArgs.Empty);
    }

    public bool IsDead() {
        return _healthAmount == 0;
    }

    public bool IsFullHealth() {
        return _healthAmount == healthAmountMax;
    }
    
    public int GetHealthAmount() {
        return _healthAmount;
    }
    
    public int GetHealthAmountMax() {
        return healthAmountMax;
    }

    public float GetHealthAmountNormalized() {
        return (float)_healthAmount / healthAmountMax;
    }

    public void SetHealthAmountMax(int amountMax, bool updateHealthAmount) {
        healthAmountMax = amountMax;

        if (updateHealthAmount) {
            _healthAmount = amountMax;
        }
    }
}

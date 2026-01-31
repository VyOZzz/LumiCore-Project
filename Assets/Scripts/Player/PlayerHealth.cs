using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public static event Action OnPlayerDeath;
    
    [Header("UI")]
    [SerializeField] private Slider healthSlider;

    private int _maxHealth = 100;
    private int _currentHealth;
    
    private void Start()
    {
        _currentHealth = _maxHealth;
        if (healthSlider != null)
        {
            healthSlider.maxValue = _maxHealth;
            healthSlider.value = _currentHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        if (healthSlider != null)
        {
            healthSlider.value = _currentHealth;
        }
        Debug.Log($"Người chơi bị trừ {damage} máu, còn {_currentHealth} máu");
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Người chơi đã chết");
        OnPlayerDeath?.Invoke();
        gameObject.SetActive(false);
        
    }
}

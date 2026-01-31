using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int maxHealth = 30;
    private int _currentHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        Debug.Log($"{name} bị trừ {damage} máu, còn {_currentHealth} máu");
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{name} đã bị tiêu diệt");
        // TODO: Sau này sẽ thêm hiệu ứng nổ, rơi đồ ở đây
        Destroy(gameObject);
    }
}

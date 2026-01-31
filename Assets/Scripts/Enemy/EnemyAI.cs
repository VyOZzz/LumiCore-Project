using System;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    
    [Header("Settings")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float detectionRange = 7f;
    
    [Header("Combat")]
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackCooldown = 1f;
    private float _lastAttackTime;
    private Transform _player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _player = Player.Instance.transform;
        if (_player == null)
        {
            Debug.LogError("Không tìm thấy Player!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(_player == null) return;
        transform.position = Vector3.MoveTowards(transform.position, _player.position, moveSpeed * Time.deltaTime);
        transform.LookAt(_player.position);
    }

    private void OnCollisionStay(Collision other)
    {
        if(other.gameObject.TryGetComponent(out PlayerHealth playerHealth))
        {
            if(Time.time >= _lastAttackTime + attackCooldown)
            {
                playerHealth.TakeDamage(damage);
                _lastAttackTime = Time.time;
            }
        }
    }
}

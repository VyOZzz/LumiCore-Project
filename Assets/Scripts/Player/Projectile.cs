using System.Collections;
using Manager;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour, IPooledObject
{
    [Header("Settings")]
    [SerializeField] private float speed = 20f;
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private int damage = 10;
    
    private Rigidbody _rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

   
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Viên đạn vừa chạm vào: " + other.name);
        if (other.TryGetComponent(out EnemyHealth enemy))
        {
            enemy.TakeDamage(damage);
        }

        ReturnToPool();
    }

    public void IncreaseDamage(int amount)
    {
        damage += amount;
        Debug.Log("Projectile damage increased by " + amount + ". New damage: " + damage);
    }
    public void IncreaseSpeedAttack(int amount)
    {
        speed += amount;
        Debug.Log("Projectile damage increased by " + amount + ". New damage: " + damage);
    }

    [Header("Pooling")]
    private Coroutine returnCoroutine;
    #region IPooledObject Implementation
    
    public void OnObjectSpawn()
    {
        //Reset velocity when respawned
        if (_rb != null)
        {
            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
            _rb.Sleep(); // Cho ngủ 1 cái để ngắt hết quán tính cũ
            _rb.WakeUp(); // Đánh thức dậy
            _rb.linearVelocity = transform.forward * speed;
            
        }
        //Stop previous return coroutine if exists
        if (returnCoroutine != null)
        {
            StopCoroutine(returnCoroutine);
        }

        returnCoroutine = StartCoroutine(ReturnToPoolAfterTime(lifeTime));
    }

    
    #endregion
    private IEnumerator ReturnToPoolAfterTime(float f)
    {
        yield return new WaitForSeconds(f);
        ReturnToPool();
    }

    private void ReturnToPool()
    {
        if(ObjectPooling.Instance == null)
        {
            Destroy(gameObject);
            return;
        }
        
        // Reset physics before returning to pool
        if (_rb != null)
        {
            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }
        
        ObjectPooling.Instance.ReturnToPool(gameObject);
    }
}

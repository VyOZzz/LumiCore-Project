using System.Collections;
using Manager;
using UnityEngine;

public class Projectile : MonoBehaviour, IPooledObject
{
    [Header("Settings")]
    [SerializeField] private float speed = 20f;
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private int damage = 10;
    
    private Coroutine _returnCoroutine;
    private bool _hasHit;
    
    void Awake()
    {
        // Set layer to PlayerProjectile để tránh va chạm với Player
        int layer = LayerMask.NameToLayer("PlayerProjectile");
        if (layer != -1)
        {
            gameObject.layer = layer;
        }
        
        // Nếu có Rigidbody, set kinematic để không bị physics can thiệp
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }

    void Update()
    {
        transform.position += transform.forward * (speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // CHỈ xử lý va chạm với Enemy, BỎ QUA tất cả thứ khác
        if (_hasHit) return;
        
        // Kiểm tra xem có phải Enemy không
        if (other.TryGetComponent(out EnemyHealth enemy))
        {
            _hasHit = true;
            if (enemy.GetCurrentHealth - damage > 0)
            {
                ObjectPooling.Instance.SpawnFromPool("VFX_ProjectileHit", transform.position, Quaternion.identity);
            }
            enemy.TakeDamage(damage);
            Debug.Log($"[Projectile] Trúng Enemy: {other.name}");
            ReturnToPool();
        }
        // Nếu không phải Enemy thì KHÔNG làm gì cả (bỏ qua Player)
    }

    public void IncreaseDamage(int amount)
    {
        damage += amount;
    }
    
    public void IncreaseSpeedAttack(int amount)
    {
        speed += amount;
    }

    #region IPooledObject Implementation
    
    public void OnObjectSpawn()
    {
        // Reset flag
        _hasHit = false;
        
        // Stop coroutine cũ nếu có
        if (_returnCoroutine != null)
        {
            StopCoroutine(_returnCoroutine);
        }
        
        // Start coroutine mới để tự động return sau lifeTime
        _returnCoroutine = StartCoroutine(ReturnToPoolAfterTime(lifeTime));
        
        Debug.Log($"[Projectile] Spawned tại {transform.position}, hướng {transform.forward}");
    }
    
    #endregion

    private IEnumerator ReturnToPoolAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        ReturnToPool();
    }

    private void ReturnToPool()
    {
        if (ObjectPooling.Instance == null)
        {
            Destroy(gameObject);
            return;
        }
        
        // Stop coroutine nếu đang chạy
        if (_returnCoroutine != null)
        {
            StopCoroutine(_returnCoroutine);
            _returnCoroutine = null;
        }
        
        // Return về pool
        ObjectPooling.Instance.ReturnToPool(gameObject);
    }
}

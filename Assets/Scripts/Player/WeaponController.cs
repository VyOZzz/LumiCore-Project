using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WeaponController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform firePoint;
    
    [Header("Stats")]
    [SerializeField] private float fireRate = 0.5f;

    [SerializeField] private float range = 10f;
    
    private float _fireTimer;
    private Transform _target;
    private readonly Collider[] _hitBuffer = new Collider[100];
    // Update is called once per frame
    void Update()
    {
        FindNearestEnemy();
        _fireTimer -= Time.deltaTime;
        if(_fireTimer <= 0f && _target != null)
        {
            Shoot();
            _fireTimer = fireRate;
        }
    }

    private void Shoot()
    {
        if(firePoint == null) return;
        
        // 1 Tính hướng bắn
        Vector3 direction = (_target.position - firePoint.position).normalized;
        // 2. Xoay họng súng về phía quái
        transform.forward = direction;
        // 3. Tạo viên đạn với rotation đúng hướng
        GameObject projectile = ObjectPooling.Instance.SpawnFromPool("Projectile", firePoint.position, Quaternion.LookRotation(direction));
        
        if (projectile == null)
        {
            Debug.LogWarning("Failed to spawn projectile from pool.");
        }
        else
        {
            Debug.Log($"Shoot direction: {direction}, Target: {_target.name}");
        }
    }

    private void FindNearestEnemy()
    {
        int hitCount = Physics.OverlapSphereNonAlloc(transform.position, range, _hitBuffer);
        Transform nearest = null;
        float minDistance = Mathf.Infinity;
        for (int i = 0; i < hitCount; i++)
        {
            if(_hitBuffer[i].TryGetComponent(out EnemyHealth _))
            {
                float distance = Vector3.Distance(transform.position, _hitBuffer[i].transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearest = _hitBuffer[i].transform;
                }
            }
        }
        _target = nearest;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}

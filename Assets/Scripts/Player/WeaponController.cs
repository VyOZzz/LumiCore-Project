using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WeaponController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject projecttilePrefab;
    [SerializeField] private Transform firePoint;
    
    [Header("Stats")]
    [SerializeField] private float fireRate = 0.5f;

    [SerializeField] private float range = 10f;
    
    private float _fireTimer;

    private Transform _target;

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
        if(firePoint == null || projecttilePrefab == null) return;
        
        // 1 Tính hướng bắn
        Vector3 direction = (_target.position - firePoint.position).normalized;
        // 2. Xoay họng súng về phía quái
        transform.forward = direction;
        // 3. Tạo viên đạn
        GameObject projectile = Instantiate(projecttilePrefab, firePoint.position, Quaternion.LookRotation(direction));
    }

    private void FindNearestEnemy()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, range);
        Transform nearest = null;
        float minDistance = Mathf.Infinity;
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out EnemyHealth enemy))
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearest = hit.transform;
                }
            }
        }
        _target = nearest;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}

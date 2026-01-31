using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float speed = 20f;
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private int damage = 10;
    
    private Rigidbody _rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.linearVelocity = transform.forward * speed;
        Destroy(gameObject, lifeTime);
    }

   
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Viên đạn vừa chạm vào: " + other.name);
        if (other.TryGetComponent(out EnemyHealth enemy))
        {
            enemy.TakeDamage(damage);
            //hủy viên đạn sau khi trúng địch
            Destroy(gameObject);
        }
        // else if (!other.CompareTag("Player")) 
        // {
        //     // Va vào tường cũng hủy
        //     Destroy(gameObject);
        // }
    }
}

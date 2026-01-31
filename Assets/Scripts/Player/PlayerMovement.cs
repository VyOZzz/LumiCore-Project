using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    
    [Header("Data")]
    [SerializeField] private PlayerStats stats;
    private Rigidbody _rb;
    private Vector3 _moveInput;

    private GameControls _input;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _input = new GameControls();
    }

    void OnEnable()
    {
        _input.GamePlay.Enable();
    }

    void OnDisable()
    {
        _input.GamePlay.Disable();
    }
   
    // Update is called once per frame
    void Update()
    {
        Vector2 inputVector2D = _input.GamePlay.Move.ReadValue<Vector2>();
        _moveInput = new Vector3(inputVector2D.x, 0, inputVector2D.y).normalized;
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
    }

    private void Move()
    {
        _rb.linearVelocity = _moveInput * stats.moveSpeed * Time.fixedDeltaTime;
    }

    private void Rotate()
    {
        if (_moveInput != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_moveInput, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, stats.rotateSpeed * Time.fixedDeltaTime);
        }
    }
  
}

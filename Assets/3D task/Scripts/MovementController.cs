using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MovementController : MonoBehaviour
{
    private CharacterController _controller;
    private Transform _cam;

    [Header("Movement Stats")]
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _turnSmoothing = 0.1f;
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private float _jumpHeight = 1.5f;

    private float _turnSmoothVelocity;
    private Vector3 _velocity;
    private bool _isGrounded;

    void Start()
    {
        _controller = GetComponent<CharacterController>();

        // Find the Main Camera (which Cinemachine is controlling)
        _cam = Camera.main.transform;

        // Hide cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        // 1. Ground Check
        _isGrounded = _controller.isGrounded;
        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        // 2. Get Input
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // 3. Move if there is input
        if (direction.magnitude >= 0.1f)
        {
            // Calculate angle to face based on Camera rotation + Input direction
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _cam.eulerAngles.y;

            // Smooth the rotation so the character doesn't snap instantly
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSmoothing);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // Convert that rotation into a movement direction
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            _controller.Move(moveDir.normalized * _speed * Time.deltaTime);
        }

        // 4. Jump
        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
        }

        // 5. Apply Gravity
        _velocity.y += _gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
    }
}
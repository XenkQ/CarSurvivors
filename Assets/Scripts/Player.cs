using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    private InputAction _moveAction;
    private InputAction _lookAction;
    private Rigidbody _rb;
    private Vector2 _moveDir;
    private Vector2 _mouseScreenPos;
    private Camera _mainCamera;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    private void Start()
    {
        _moveAction = InputSystem.actions.FindAction("Move");
        _lookAction = InputSystem.actions.FindAction("Look");
    }

    private void Update()
    {
        _moveDir = _moveAction.ReadValue<Vector2>().normalized;
        _mouseScreenPos = _lookAction.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        Move();

        RotateInMouseDirection();
    }

    private void Move()
    {
        _rb.linearVelocity = _moveSpeed * Time.deltaTime * new Vector3(_moveDir.x, 0, _moveDir.y);
    }

    private void RotateInMouseDirection()
    {
        Ray ray = _mainCamera.ScreenPointToRay(_mouseScreenPos);
        if(Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            Vector3 flatPosition = new(transform.position.x, 0, transform.position.z);
            Vector3 flatPoint = new(hitInfo.point.x, 0, hitInfo.point.z);
            Vector3 rotationDir = flatPosition - flatPoint;
            float angle = Mathf.Atan2(rotationDir.z, rotationDir.x) * Mathf.Rad2Deg;
            Debug.DrawLine(flatPosition, flatPoint);
            Debug.Log($"{flatPoint} {flatPosition}");
            _rb.MoveRotation(Quaternion.AngleAxis(angle, -Vector3.up));
        }
    }
}

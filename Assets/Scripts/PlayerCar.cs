using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerCar : MonoBehaviour
{
    private enum Axel
    {
        Front,
        Rear
    }

    [Serializable]
    private struct Wheel
    {
        public GameObject wheelModel;
        public WheelCollider wheelCollider;
        public Axel axel;
    }

    [Header("Speed")]
    [SerializeField] private float _speed = 600f;
    [SerializeField] private float _maxAcceleration = 30.0f;
    [SerializeField] private float _brakeAcceleration = 50.0f;

    [Header("Rotation")]
    [SerializeField] private float _turnSensitivity = 1.0f;
    [SerializeField] private float _maxSteerAngle = 30.0f;

    [Header("Physics")]
    [SerializeField] private Vector3 _centerOfMass;
    private Rigidbody _rb;

    [SerializeField] private List<Wheel> _wheels;
    
    private InputAction _moveAction;
    private Vector2 _moveInput;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _moveAction = InputSystem.actions.FindAction("Move");
        _rb.centerOfMass = _centerOfMass;
    }

    private void Update()
    {
        _moveInput = _moveAction.ReadValue<Vector2>().normalized;
    }

    private void FixedUpdate()
    {
        Move();
        Steer();
        AnimateWheels();
    }

    private void Move()
    {
        foreach (var wheel in _wheels)
        {
            wheel.wheelCollider.motorTorque = _moveInput.y * _speed * _maxAcceleration * Time.deltaTime;
        }
    }

    private void Steer()
    {
        foreach(var wheel in _wheels)
        {
            if(wheel.axel == Axel.Front)
            {
                float steerAngle = _moveInput.x * _turnSensitivity * _maxSteerAngle;
                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, steerAngle, 0.6f);
            }
        }
    }


    void AnimateWheels()
    {
        foreach (var wheel in _wheels)
        {
            wheel.wheelCollider.GetWorldPose(out Vector3 pos, out Quaternion rot);
            wheel.wheelModel.transform.SetPositionAndRotation(pos, rot);
        }
    }
}

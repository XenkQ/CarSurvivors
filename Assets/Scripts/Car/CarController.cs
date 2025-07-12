using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Car
{
    public interface ICarController
    {
        event EventHandler OnBrakePress;

        event EventHandler OnBrakeRelease;

        float GetMovementSpeed();
    }

    [RequireComponent(typeof(Rigidbody))]
    public class CarController : MonoBehaviour, ICarController
    {
        private enum Axel
        {
            Front,
            Rear
        }

        [Serializable]
        private struct Wheel
        {
            public GameObject WheelModel;
            public WheelCollider WheelCollider;
            public Axel Axel;
        }

        [Header("BulletTimeToArriveAtRangeEnd")]
        [SerializeField] private float _speed = 600f;
        [SerializeField] private float _maxAcceleration = 30.0f;

        [Header("Rotation")]
        [SerializeField] private float _turnSensitivity = 1.0f;
        [SerializeField] private float _maxSteerAngle = 30.0f;

        [Header("Physics")]
        [SerializeField] private Vector3 _centerOfMass;
        private Rigidbody _rb;

        [SerializeField] private List<Wheel> _wheels;

        public event EventHandler OnBrakePress;

        public event EventHandler OnBrakeRelease;

        private InputAction _moveAction;
        private Vector2 _moveInput;

        private InputAction _brakeAction;
        private bool _brakeInput;
        private float _brakeTorqueMultiplier = 1000f;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            _moveAction = InputSystem.actions.FindAction("Move");
            _brakeAction = InputSystem.actions.FindAction("Brake");
            _rb.centerOfMass = _centerOfMass;

            _brakeAction.started += (ctx) => OnBrakePress.Invoke(this, EventArgs.Empty);
            _brakeAction.canceled += (ctx) => OnBrakeRelease.Invoke(this, EventArgs.Empty);
        }

        private void Update()
        {
            _moveInput = _moveAction.ReadValue<Vector2>().normalized;
            _brakeInput = _brakeAction.IsPressed();
        }

        private void FixedUpdate()
        {
            HandleMove();
            HandleSteer();
            HandleBrake();
            AnimateWheels();
        }

        public float GetMovementSpeed()
        {
            return _rb.linearVelocity.magnitude;
        }

        private void HandleMove()
        {
            foreach (var wheel in _wheels)
            {
                wheel.WheelCollider.motorTorque = _moveInput.y * _speed * _maxAcceleration * Time.deltaTime;
            }
        }

        private void HandleSteer()
        {
            foreach (var wheel in _wheels)
            {
                if (wheel.Axel == Axel.Front)
                {
                    float steerAngle = _moveInput.x * _turnSensitivity * _maxSteerAngle;
                    wheel.WheelCollider.steerAngle = Mathf.Lerp(wheel.WheelCollider.steerAngle, steerAngle, 0.6f);
                }
            }
        }

        private void HandleBrake()
        {
            float brakeTorque = _brakeInput ? _maxAcceleration * _brakeTorqueMultiplier : 0f;
            foreach (var wheel in _wheels)
            {
                wheel.WheelCollider.brakeTorque = brakeTorque;
            }
        }

        private void AnimateWheels()
        {
            foreach (var wheel in _wheels)
            {
                wheel.WheelCollider.GetWorldPose(out Vector3 pos, out Quaternion rot);
                wheel.WheelModel.transform.SetPositionAndRotation(pos, rot);
            }
        }
    }
}

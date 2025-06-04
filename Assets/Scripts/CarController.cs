using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class CarController : MonoBehaviour
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
                wheel.WheelCollider.motorTorque = _moveInput.y * _speed * _maxAcceleration * Time.deltaTime;
            }
        }

        private void Steer()
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

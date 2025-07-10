using Assets.Scripts.CustomTypes;
using Assets.Scripts.Extensions;
using Assets.Scripts.FlowFieldSystem;
using Assets.Scripts.Player;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.LevelSystem.Exp
{
    public interface IExpParticle
    {
        public event EventHandler OnExpReachedTarget;

        public void SetSizeAndMaterialBasedOnExpAmount(float exp);

        public void PlayDisapearingAnimation(TweenCallback callback);
    }

    [RequireComponent(typeof(FlowFieldMovementController))]
    public class ExpParticle : MonoBehaviour, IExpParticle
    {
        [Serializable]
        private struct ExpParticleApearanceByTreshold
        {
            public float Treshold;
            public Material Material;
            public FloatValueRange ScaleValueRange;
        }

        [SerializeField] private float _movementSpeed;
        [SerializeField] private float _rangeToChannelExp;
        [SerializeField] private float _inChannelRangeCheckDelay = 0.2f;
        [SerializeField] private float _disapearingDuration = 0.1f;

        [SerializeField] private GameObject _visual;
        [SerializeField] private ExpParticleApearanceByTreshold[] _particleApearanceByTreshold;

        public event EventHandler OnExpReachedTarget;

        private float _expAmount;
        private bool _expChanneled;

        private IFlowFieldMovementController _flowFieldMovementController;
        private PlayerManager _playerManager;

        private void Awake()
        {
            _flowFieldMovementController = GetComponent<IFlowFieldMovementController>();
        }

        private void Start()
        {
            _playerManager = PlayerManager.Instance;
        }

        private void OnEnable()
        {
            _expChanneled = false;
            _expAmount = 0;
            InvokeRepeating(nameof(ChannelExpToPlayerWhenInChannelRange), 0, _inChannelRangeCheckDelay);
        }

        private void OnDisable()
        {
            CancelInvoke(nameof(ChannelExpToPlayerWhenInChannelRange));
        }

        private void FixedUpdate()
        {
            _flowFieldMovementController.MoveOnFlowFieldGrid(_movementSpeed);
        }

        private void OnDrawGizmos()
        {
            new Debug().DrawCircle(transform.position, _rangeToChannelExp);
        }

        public void SetSizeAndMaterialBasedOnExpAmount(float exp)
        {
            _expAmount = exp;

            foreach (var expTresholdMaterial in _particleApearanceByTreshold)
            {
                if (exp < expTresholdMaterial.Treshold)
                {
                    MeshRenderer meshRenderer = _visual.GetComponent<MeshRenderer>();
                    meshRenderer.SetMaterials(new List<Material>() { expTresholdMaterial.Material });
                    _visual.transform.localScale = Vector3.one * expTresholdMaterial.ScaleValueRange.GetRandomValueInRange();
                    break;
                }
            }
        }

        public void PlayDisapearingAnimation(TweenCallback callback)
        {
            transform.LifeEndingShrinkToZeroTween(_disapearingDuration, callback);
        }

        private void ChannelExpToPlayerWhenInChannelRange()
        {
            if (!_expChanneled
                && Vector3.Distance(_playerManager.transform.position, transform.position) <= _rangeToChannelExp)
            {
                _playerManager.LevelController.AddExp(_expAmount);
                _expChanneled = true;
                OnExpReachedTarget.Invoke(this, EventArgs.Empty);
            }
        }
    }
}

﻿using Assets.Scripts.Audio;
using Assets.Scripts.CustomTypes;
using Assets.Scripts.Extensions;
using Assets.Scripts.FlowFieldSystem;
using Assets.Scripts.LayerMasks;
using Assets.Scripts.Player;
using Assets.Scripts.Providers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.LevelSystem.Exp
{
    public interface IExpParticle : IGameObjectProvider
    {
        public event EventHandler OnExpReachedTarget;

        public void SetSizeAndMaterialBasedOnExpAmount(float exp);

        public void CollectExp(Action callback);
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
        [SerializeField] private float _disapearingDuration = 0.1f;

        [SerializeField] private GameObject _visual;
        [SerializeField] private ExpParticleApearanceByTreshold[] _particleApearanceByTreshold;

        public GameObject GameObject => gameObject;

        public event EventHandler OnExpReachedTarget;

        private float _expAmount;
        private bool _expCollected;

        private IFlowFieldMovementController _flowFieldMovementController;

        private IAudioClipPlayer _audoClipPlayer;

        private void Awake()
        {
            _flowFieldMovementController = GetComponent<IFlowFieldMovementController>();
            _audoClipPlayer = GetComponentInChildren<IAudioClipPlayer>();
        }

        private void OnEnable()
        {
            _expCollected = false;
            _expAmount = 0;
        }

        private void FixedUpdate()
        {
            _flowFieldMovementController.MoveOnFlowFieldGrid(_movementSpeed);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_expCollected && (1 << other.gameObject.layer) == EntityLayers.Player)
            {
                OnExpReachedTarget?.Invoke(this, EventArgs.Empty);
            }
        }

        public void SetSizeAndMaterialBasedOnExpAmount(float exp)
        {
            if (_particleApearanceByTreshold.Length == 0)
            {
                Debug.LogError("Particle apearance by treshold is not set set for: " + transform.name);
            }

            _expAmount = exp;

            for (int i = _particleApearanceByTreshold.Length - 1; i >= 0; i--)
            {
                if (_particleApearanceByTreshold[i].Treshold <= exp)
                {
                    MeshRenderer meshRenderer = _visual.GetComponent<MeshRenderer>();
                    meshRenderer.SetMaterials(new List<Material>() { _particleApearanceByTreshold[i].Material });
                    transform.localScale =
                        Vector3.one * _particleApearanceByTreshold[i].ScaleValueRange.GetRandomValueInRange();

                    break;
                }
            }
        }

        public void CollectExp(Action callback = null)
        {
            bool audioClipPlayFinished = false;
            _audoClipPlayer.Play("ExpCollected");

            _audoClipPlayer.OnAudioClipFinished += (s, e) => audioClipPlayFinished = true;

            transform.LifeEndingShrinkToZeroTween(_disapearingDuration, () =>
            {
                PlayerManager.Instance.LevelController.AddExp(_expAmount);

                if (audioClipPlayFinished)
                {
                    callback?.Invoke();
                }
                else
                {
                    _audoClipPlayer.OnAudioClipFinished += (s, e) => callback?.Invoke();
                }
            });
        }
    }
}

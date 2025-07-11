using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.LevelSystem.Exp
{
    public sealed class ExpParticleSpawner : MonoBehaviour
    {
        [Serializable]
        public struct ExpParticleSpawnData
        {
            public Vector3 Pos;
            public float Exp;

            public ExpParticleSpawnData(Vector3 pos, float exp)
            {
                Pos = pos;
                Exp = exp;
            }
        }

        [Serializable]
        private struct ExpTresholdDevider
        {
            public float Treshold;
            public float Divider;
        }

        [SerializeField] private float _particlesYOffset;
        [SerializeField] private ExpParticle _expParticlePrefab;
        [SerializeField] private ExpTresholdDevider[] _expTresholdDeviders;
        [SerializeField, Range(0, 30f)] private float _spawningCircleRadius;

        private const float CHECK_QUEUED_EXP_SPAWNS_DELAY = 0.2f;
        public static ExpParticleSpawner Instance;
        private Queue<ExpParticleSpawnData> _queuedExpSpawns = new();

        private void Awake()
        {
            if (Instance is null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            InvokeRepeating(
                nameof(SpawnParticlesBasedOnExpAmount),
                CHECK_QUEUED_EXP_SPAWNS_DELAY,
                CHECK_QUEUED_EXP_SPAWNS_DELAY
            );
        }

        private ExpParticleSpawner()
        { }

        public void SpawnExpParticle(ExpParticleSpawnData expParticleSpawnData)
        {
            _queuedExpSpawns.Enqueue(expParticleSpawnData);
        }

        private void SpawnParticlesBasedOnExpAmount()
        {
            if (_expTresholdDeviders.Length == 0)
            {
                Debug.LogError("Exp treshold deviders not set for: " + transform.name);
            }

            if (_queuedExpSpawns.Count == 0)
            {
                return;
            }

            ExpTresholdDevider[] reversedExpTresholdDeviders = _expTresholdDeviders.Reverse().ToArray();

            while (_queuedExpSpawns.Count > 0)
            {
                ExpParticleSpawnData expParticleSpawnData = _queuedExpSpawns.Dequeue();
                float exp = expParticleSpawnData.Exp;
                Vector3 pos = expParticleSpawnData.Pos;

                ExpTresholdDevider expTresholdDevider =
                    reversedExpTresholdDeviders.FirstOrDefault(etd => etd.Treshold <= exp);

                float expPart = exp / expTresholdDevider.Divider;
                for (int i = 0; i < expTresholdDevider.Divider; i++)
                {
                    Vector3 randomPos = RandomUtility.GetRandomPositionFromCircle(pos, _spawningCircleRadius);
                    randomPos.y = _particlesYOffset;

                    IExpParticle expParticle = Instantiate(_expParticlePrefab, randomPos, Quaternion.identity);
                    expParticle.SetSizeAndMaterialBasedOnExpAmount(exp);
                    expParticle.OnExpReachedTarget += ExpParticle_OnExpReachedTarget;
                }
            }
        }

        private void ExpParticle_OnExpReachedTarget(object sender, EventArgs e)
        {
            if (sender is IExpParticle expParticle)
            {
                expParticle.PlayDisapearingAnimation(() => Destroy(expParticle.GameObject));
                expParticle.OnExpReachedTarget -= ExpParticle_OnExpReachedTarget;
            }
        }
    }
}

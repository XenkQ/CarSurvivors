using Assets.Scripts.Enemies;
using System.Linq;
using UnityEngine;

namespace Scripts
{
    public class WaveManager : MonoBehaviour
    {
        [SerializeField] private float _startSpawnWaveDelay = 8f;
        private float _currentSpawnWaveDelay;
        private float _firstWaveDelay = 1f;
        private ushort _maxEnemiesInWave = 4;
        private float _maxEnemiesInWaveMultiplier = 1.2f;
        private ushort _wave = 1;

        private void Start()
        {
            _currentSpawnWaveDelay = _firstWaveDelay;
        }

        private void Update()
        {
            WavesProcess();
        }

        private void WavesProcess()
        {
            if ((_wave == 1 || EnemiesSpawner.Instance.SpawnedEnemiesCounter > 0) && _currentSpawnWaveDelay > 0)
            {
                _currentSpawnWaveDelay -= Time.deltaTime;
            }
            else
            {
                SpawnWave();
                _currentSpawnWaveDelay = _startSpawnWaveDelay;
            }
        }

        private void SpawnWave()
        {
            EnemiesSpawner.Instance.SpawnRandomEnemiesBasedOnSpawnChance(_maxEnemiesInWave);
            float maxEnemiesInWave = _maxEnemiesInWave * _maxEnemiesInWaveMultiplier;
            if (maxEnemiesInWave < ushort.MaxValue)
            {
                _maxEnemiesInWave = (ushort)(_maxEnemiesInWave * _maxEnemiesInWaveMultiplier);
            }
            else
            {
                _maxEnemiesInWave = ushort.MaxValue;
            }
            _wave++;
        }
    }
}

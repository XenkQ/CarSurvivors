using Assets.Scripts.GridSystem;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class EnemiesOutsidePlayerChunkTeleporter : MonoBehaviour
    {
        [SerializeField] private Transform _enemiesHolder;
        [SerializeField] private float _checkForEnemiesOutsidePlayerChunkDelay = 2f;

        private void Start()
        {
            InvokeRepeating(
                nameof(TeleportEnemiesFromOutsideToInsidePlayerChunk),
                _checkForEnemiesOutsidePlayerChunkDelay,
                _checkForEnemiesOutsidePlayerChunkDelay);
        }

        public void TeleportEnemiesFromOutsideToInsidePlayerChunk()
        {
            foreach (Enemy enemy in GetEnemiesOutsidePlayerChunk())
            {
                enemy.transform.position = RandomGridCellWithConditionFinder
                    .GetRandomWalkableEdgeCellOrNull(GridManager.Instance.GridPlayerChunk)
                    .WorldPos;
            }
        }

        private Enemy[] GetEnemiesOutsidePlayerChunk()
        {
            List<Enemy> enemies = new List<Enemy>();

            GridSystem.Grid playerChunk = GridManager.Instance.GridPlayerChunk;
            Cell centerCell = playerChunk.Cells[playerChunk.Width / 2, playerChunk.Height / 2];
            Vector3 center = centerCell.WorldPos;

            float width = playerChunk.Width * 0.5f * playerChunk.CellSize;
            float height = playerChunk.Height * 0.5f * playerChunk.CellSize;

            foreach (Transform child in _enemiesHolder)
            {
                if (child.TryGetComponent(out Enemy enemy))
                {
                    Vector3 enemyPos = enemy.transform.position;
                    if (Mathf.Abs(enemyPos.x - center.x) > width || Mathf.Abs(enemyPos.z - center.z) > height)
                    {
                        enemies.Add(enemy);
                    }
                }
            }

            return enemies.ToArray();
        }
    }
}

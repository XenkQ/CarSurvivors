using System;
using System.Linq;
using Assets.Scripts.GridSystem;
using UnityEngine;

namespace Assets.Scripts.Skills.ObjectsImpactingSkills.Crate
{
    public class SkillCrateSpawner : MonoBehaviour
    {
        private const byte MAX_SPAWN_COUNT = 5;
        private const float CRATE_Y_OFFSET = 0.925f;

        [field: SerializeField] public float SpawnDelay { get; set; } = 10f;
        [SerializeField] private SkillCrate _skillCratePrefab;
        private byte _spawnCount;

        private SkillCrate[] _spawnedSkillCrates = new SkillCrate[MAX_SPAWN_COUNT];

        public void Start()
        {
            InvokeRepeating(nameof(SpawnCrateAtRandomNotOccupiedCell), SpawnDelay, SpawnDelay);
        }

        private void SpawnCrateAtRandomNotOccupiedCell()
        {
            if (_spawnCount < MAX_SPAWN_COUNT)
            {
                Cell drawnCell = RandomGridCellWithConditionFinder
                    .FindRandomWalkableCellWithoutCollectibleOrNull(GridManager.Instance.WorldGrid);

                if (drawnCell == null)
                {
                    Debug.LogError("No walkable cell without collectible found for spawning a skill crate.");
                    return;
                }

                SkillCrate skillCrate = Instantiate(
                    _skillCratePrefab,
                    new Vector3(drawnCell.WorldPos.x, CRATE_Y_OFFSET, drawnCell.WorldPos.z),
                    Quaternion.identity,
                    transform);

                skillCrate.OnCollected += SkillCrate_OnCollected;

                _spawnedSkillCrates[_spawnCount++] = skillCrate;
                drawnCell.IsOccupiedByCollectible = true;
            }
        }

        private void SkillCrate_OnCollected(object sender, EventArgs e)
        {
            SkillCrate skillCrate = sender as SkillCrate;

            if (skillCrate != null)
            {
                _spawnedSkillCrates = _spawnedSkillCrates
                    .Where(c => c != skillCrate)
                    .ToArray();
            }
        }
    }
}

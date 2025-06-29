using Assets.ScriptableObjects.Player.Skills;
using Assets.Scripts;
using Assets.Scripts.CustomTypes;
using UnityEngine;

namespace Assets.ScriptableObjects.Skills.PlayerSkills.LandmineSkill
{
    [CreateAssetMenu(fileName = "LandmineSkillSO", menuName = "Scriptable Objects/Skills/LandmineSkillSO")]
    public class LandmineSkillUpgradeableConfigSO : SkillUpgradeableStatsConfig
    {
        [field: SerializeField] public float ThrowLandmineRange { get; private set; }
        [field: SerializeField] public float TimeToReachThrowRangeEnd { get; private set; }
        [SerializeField] private FloatUpgradeableStat _spawnCooldown;
        [SerializeField] private FloatUpgradeableStat _explosionRadius;
        [SerializeField] private FloatUpgradeableStat _damage;
        [SerializeField] private FloatUpgradeableStat _size;

        public FloatUpgradeableStat SpawnCooldown { get; private set; }
        public FloatUpgradeableStat ExplosionRadius { get; private set; }
        public FloatUpgradeableStat Damage { get; private set; }
        public FloatUpgradeableStat Size { get; private set; }

        private void OnEnable()
        {
            SpawnCooldown = DeepCopyUtility.DeepCopy(_spawnCooldown);
            ExplosionRadius = DeepCopyUtility.DeepCopy(_explosionRadius);
            Damage = DeepCopyUtility.DeepCopy(_damage);
            Size = DeepCopyUtility.DeepCopy(_size);
        }
    }
}

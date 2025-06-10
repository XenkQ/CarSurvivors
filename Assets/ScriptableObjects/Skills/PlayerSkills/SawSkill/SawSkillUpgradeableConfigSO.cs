using Assets.ScriptableObjects.Player.Skills;
using Assets.Scripts;
using Assets.Scripts.CustomTypes;
using UnityEngine;

namespace Assets.ScriptableObjects.Skills.PlayerSkills.SawSkill
{
    [CreateAssetMenu(fileName = "SawSkillSO", menuName = "Scriptable Objects/Skills/SawSkillSO")]
    public class SawSkillUpgradeableConfigSO : SkillUpgradeableStatsConfig
    {
        [field: SerializeField] public float TimeToArriveAtKnockbackLocation { get; private set; }
        [field: SerializeField] public float AttackCooldown { get; private set; } = 0.05f;
        [SerializeField] private FloatUpgradeableStat _knockbackPower;
        [SerializeField] private FloatUpgradeableStat _damage;
        [SerializeField] private FloatUpgradeableStat _size;

        public FloatUpgradeableStat KnockbackPower { get; private set; }
        public FloatUpgradeableStat Damage { get; private set; }
        public FloatUpgradeableStat Size { get; private set; }

        private void OnEnable()
        {
            KnockbackPower = DeepCopyUtility.DeepCopy(_knockbackPower);
            Damage = DeepCopyUtility.DeepCopy(_damage);
            Size = DeepCopyUtility.DeepCopy(_size);
        }
    }
}

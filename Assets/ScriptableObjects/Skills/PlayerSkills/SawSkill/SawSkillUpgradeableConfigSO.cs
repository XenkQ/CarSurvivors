using Assets.ScriptableObjects.Player.Skills;
using Assets.Scripts;
using Assets.Scripts.Skills;
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
        [SerializeField] private ByteUpgradeableStat _numberOfSaws;

        public FloatUpgradeableStat KnockbackPower { get; private set; }
        public FloatUpgradeableStat Damage { get; private set; }
        public ByteUpgradeableStat NuberOfSaws { get; private set; }

        private void OnEnable()
        {
            KnockbackPower = DeepCopyUtility.DeepCopy(_knockbackPower);
            Damage = DeepCopyUtility.DeepCopy(_damage);
            NuberOfSaws = DeepCopyUtility.DeepCopy(_numberOfSaws);
        }
    }
}

using Assets.ScriptableObjects.Player.Skills;
using Assets.Scripts.CustomTypes;
using UnityEngine;

[CreateAssetMenu(fileName = "SawSkillSO", menuName = "Scriptable Objects/Skills/SawSkillSO")]
public class SawSkillUpgradeableConfigSO : SkillUpgradeableStatsConfig
{
    [SerializeField] private FloatUpgradeableStat _knockbackPower;
    [SerializeField] private FloatUpgradeableStat _timeToArriveAtKnockbackLocation;
    [SerializeField] private FloatUpgradeableStat _damage;
    [SerializeField] private FloatUpgradeableStat _size;
    [SerializeField] private FloatUpgradeableStat _attackCooldown;

    public FloatUpgradeableStat KnockbackPower { get; private set; }
    public FloatUpgradeableStat TimeToArriveAtKnockbackLocation { get; private set; }
    public FloatUpgradeableStat Damage { get; private set; }
    public FloatUpgradeableStat Size { get; private set; }
    public FloatUpgradeableStat AttackCooldown { get; private set; }

    private void OnEnable()
    {
        KnockbackPower = _knockbackPower;
        TimeToArriveAtKnockbackLocation = _timeToArriveAtKnockbackLocation;
        Damage = _damage;
        Size = _size;
        AttackCooldown = _attackCooldown;
    }
}

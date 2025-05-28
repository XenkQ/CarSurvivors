using Assets.ScriptableObjects.Player.Skills;
using Assets.Scripts.CustomTypes;
using UnityEngine;

[CreateAssetMenu(fileName = "SawSkillSO", menuName = "Scriptable Objects/Skills/SawSkillSO")]
public class SawSkillUpgradableConfigSO : SkillUpgradableConfig
{
    [SerializeField] private FloatUpgradableStat _knockbackPower;
    [SerializeField] private FloatUpgradableStat _timeToArriveAtKnockbackLocation;
    [SerializeField] private FloatUpgradableStat _damage;
    [SerializeField] private FloatUpgradableStat _size;
    [SerializeField] private FloatUpgradableStat _attackCooldown;
    [SerializeField] private ByteUpgradableStat _level;

    public FloatUpgradableStat KnockbackPower { get; private set; }
    public FloatUpgradableStat TimeToArriveAtKnockbackLocation { get; private set; }
    public FloatUpgradableStat Damage { get; private set; }
    public FloatUpgradableStat Size { get; private set; }
    public FloatUpgradableStat AttackCooldown { get; private set; }
    public ByteUpgradableStat Level { get; private set; }

    private void OnEnable()
    {
        KnockbackPower = _knockbackPower;
        TimeToArriveAtKnockbackLocation = _timeToArriveAtKnockbackLocation;
        Damage = _damage;
        Size = _size;
        AttackCooldown = _attackCooldown;
        Level = _level;
    }
}

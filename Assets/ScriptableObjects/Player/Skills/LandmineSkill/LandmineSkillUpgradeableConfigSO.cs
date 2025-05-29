using Assets.ScriptableObjects.Player.Skills;
using Assets.Scripts.CustomTypes;
using UnityEngine;

[CreateAssetMenu(fileName = "LandmineSkillSO", menuName = "Scriptable Objects/Skills/LandmineSkillSO")]
public class LandmineSkillUpgradeableConfigSO : SkillUpgradeableConfig
{
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
        SpawnCooldown = _spawnCooldown;
        ExplosionRadius = _explosionRadius;
        Damage = _damage;
        Size = _size;
    }
}

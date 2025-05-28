using Assets.ScriptableObjects.Player.Skills;
using Assets.Scripts.CustomTypes;
using UnityEngine;

[CreateAssetMenu(fileName = "LandmineSkillSO", menuName = "Scriptable Objects/Skills/LandmineSkillSO")]
public class LandmineSkillConfigSO : SkillConfig
{
    [SerializeField] private FloatUpgradableStat _spawnCooldown;
    [SerializeField] private FloatUpgradableStat _explosionRadius;
    [SerializeField] private FloatUpgradableStat _damage;
    [SerializeField] private FloatUpgradableStat _size;
    [SerializeField] private ByteUpgradableStat _level;

    public FloatUpgradableStat SpawnCooldown { get; private set; }
    public FloatUpgradableStat ExplosionRadius { get; private set; }
    public FloatUpgradableStat Damage { get; private set; }
    public FloatUpgradableStat Size { get; private set; }
    public ByteUpgradableStat Level { get; private set; }

    private void OnEnable()
    {
        SpawnCooldown = _spawnCooldown;
        ExplosionRadius = _explosionRadius;
        Damage = _damage;
        Size = _size;
        Level = _level;
    }
}

using Assets.ScriptableObjects.Player.Skills;
using UnityEngine;

[CreateAssetMenu(fileName = "LandmineSkillSO", menuName = "Scriptable Objects/Skills/LandmineSkillSO")]
public class LandmineSkillConfigSO : SkillConfig
{
    public float SpawnCooldown;
    public float ExplosionRadius;
    public float Damage;
    public float Size;
    public byte Level;
}

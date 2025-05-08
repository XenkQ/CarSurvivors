using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileStatsSO", menuName = "Scriptable Objects/ProjectileStatsSO")]
public class ProjectileStatsSO : ScriptableObject
{
    public float Damage;
    public float Size;
    public float TimeToArriveAtRangeEnd;
    public float Range;
    public ushort MaxPiercing;
}

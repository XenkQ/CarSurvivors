using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileStatsSO", menuName = "Scriptable Objects/ProjectileStatsSO")]
public class ProjectileStatsSO : ScriptableObject
{
    [SerializeField] private float _startDamage;
    [SerializeField] private float _startSize;
    [SerializeField] private float _startTimeToArriveAtRangeEnd;
    [SerializeField] private float _startRange;
    [SerializeField] private ushort _startMaxPiercing;

    public float Damage { get; set; }
    public float Size { get; set; }
    public float TimeToArriveAtRangeEnd { get; set; }
    public float Range { get; set; }
    public ushort MaxPiercing { get; set; }

    private void OnEnable()
    {
        Damage = _startDamage;
        Size = _startSize;
        TimeToArriveAtRangeEnd = _startTimeToArriveAtRangeEnd;
        Range = _startRange;
        MaxPiercing = _startMaxPiercing;
    }
}

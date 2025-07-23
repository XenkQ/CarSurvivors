using UnityEngine;

namespace Assets.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ProjectileStatsSO", menuName = "Scriptable Objects/ProjectileStatsSO")]
    public class ProjectileConfigSO : ScriptableObject
    {
        [field: SerializeField] public float DisapearingDuration { get; private set; } = 0.1f;
        [SerializeField] private byte _startDamage;
        [SerializeField] private float _startSize;
        [SerializeField] private float _startSpeed;
        [SerializeField] private float _startRange;
        [SerializeField] private byte _startMaxPiercing;

        public byte Damage { get; set; }
        public float Speed { get; set; }
        public float Size { get; set; }
        public float Range { get; set; }
        public byte MaxPiercing { get; set; }

        private void OnEnable()
        {
            Damage = _startDamage;
            Speed = _startSpeed;
            Size = _startSize;
            Range = _startRange;
            MaxPiercing = _startMaxPiercing;
        }
    }
}

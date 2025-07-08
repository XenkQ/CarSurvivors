using Assets.Scripts.Initializers;
using UnityEngine;

namespace Assets.Scripts.Skills
{
    public abstract class Turret<ConfigSO> : MonoBehaviour, IInitializableWithScriptableConfig<ConfigSO>
        where ConfigSO : ScriptableObject
    {
        [field: SerializeField] protected ConfigSO _config { get; set; }
        [field: SerializeField] protected Transform _gunTip;
        [SerializeField] protected Transform _visual;
        [SerializeField] protected Projectile _turretsProejctile;
        protected Transform _projectilesParent;

        protected virtual void Awake()
        {
            _projectilesParent = GameObject.FindGameObjectWithTag("ProjectilesHolder")?.transform;
        }

        public abstract void Shoot(float shootPreparingAnimationSpeed = 1f);

        public virtual void Initialize(ConfigSO config)
        {
            _config = config;
            gameObject.SetActive(true);
        }

        public bool IsInitialized()
        {
            return gameObject.activeSelf && _config is not null;
        }
    }
}

using Assets.Scripts.StatusAffectables;
using DG.Tweening;
using UnityEngine;
using VFX;

namespace Assets.Scripts.Player
{
    [RequireComponent(typeof(PlayerManager))]
    public class PlayerDamagedHandler : MonoBehaviour, IDamageable
    {
        [Header("Needed references")]
        [SerializeField] private VFXPlayer _damageVfxPlayer;
        [SerializeField] private GameObject _carVisual;

        [Header("Shake after damage settings")]
        [SerializeField] private float _duration = 0.2f;
        [SerializeField] private float _strength = 0.1f;

        private PlayerManager _playerManager;
        private Tween _shakeTween;

        private void Start()
        {
            _playerManager = PlayerManager.Instance;
        }

        public void TakeDamage(float damage)
        {
            _playerManager.Health.DecreaseHealth(damage);
            _playerManager.AudioClipPlayer.PlayOneShot("Damaged");
            _damageVfxPlayer.Play(new());

            if (_shakeTween?.IsPlaying() ?? false)
            {
                _shakeTween.Complete();
                _shakeTween.Kill();
            }

            _shakeTween = _carVisual.transform.DOShakeScale(_duration, _strength);
        }

        public void TakeFullHpDamage()
        {
            _playerManager.Health.DecreaseHealth(_playerManager.Health.MaxHealth);
        }
    }
}

using Assets.Scripts.UI.Death;
using Assets.Scripts.VFX;
using Reflex.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Player
{
    [RequireComponent(typeof(PlayerManager))]
    public class PlayerDeathHandler : MonoBehaviour
    {
        [Inject] private readonly IPlayerManager _playerManager;
        [Inject] private readonly IPlayerDeathPresenter _playerDeathPresenter;

        [SerializeField] private GameObject _visual;
        [SerializeField] private VFXPlayer _deathVfxPlayer;
        [SerializeField] private Collider[] _wheelColliders;
        private Collider[] _allColliders;

        private void Awake()
        {
            _allColliders = GetComponentsInChildren<Collider>(true);
        }

        private void OnEnable()
        {
            _playerManager.Health.OnNoHealth += Health_OnNoHealth;
            _deathVfxPlayer.OnVFXFinished += DeathVfxPlayer_OnVFXFinished;
        }

        private void OnDisable()
        {
            _playerManager.Health.OnNoHealth -= Health_OnNoHealth;
            _deathVfxPlayer.OnVFXFinished -= DeathVfxPlayer_OnVFXFinished;
        }

        private void Health_OnNoHealth(object sender, EventArgs e)
        {
            _visual.SetActive(false);

            DisableNotWheelColliders();

            _deathVfxPlayer.Play(new VFXPlayConfig());
        }

        private void DeathVfxPlayer_OnVFXFinished(object sender, EventArgs e)
        {
            _playerDeathPresenter.EnableDeathScreen();
        }

        private void DisableNotWheelColliders()
        {
            IEnumerable<Collider> notWheelColliders = _allColliders.Where(aC => !_wheelColliders.Any(wC => wC == aC));
            foreach (Collider collider in notWheelColliders)
            {
                collider.enabled = false;
            }
        }
    }
}

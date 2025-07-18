using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VFX;

namespace Assets.Scripts.Player
{
    [RequireComponent(typeof(PlayerManager))]
    public class PlayerDeathHandler : MonoBehaviour
    {
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
            PlayerManager.Instance.Health.OnNoHealth += Health_OnNoHealth;
            _deathVfxPlayer.OnVFXFinished += DeathVfxPlayer_OnVFXFinished;
        }

        private void OnDisable()
        {
            PlayerManager.Instance.Health.OnNoHealth -= Health_OnNoHealth;
            _deathVfxPlayer.OnVFXFinished -= DeathVfxPlayer_OnVFXFinished;
        }

        private void Health_OnNoHealth(object sender, EventArgs e)
        {
            _visual.SetActive(false);
            Debug.Log("ON NO HEALTH");

            DisableNotWheelColliders();

            _deathVfxPlayer.Play(new VFXPlayConfig());
        }

        private void DeathVfxPlayer_OnVFXFinished(object sender, EventArgs e)
        {
            Debug.Log("VFX FINISHED");
            PlayerDeathPresenter.Instace.EnableDeathScreen();
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

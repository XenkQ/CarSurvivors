using System;
using UnityEngine;
using VFX;

namespace Assets.Scripts.Player
{
    [RequireComponent(typeof(PlayerManager))]
    public class PlayerDeathHandler : MonoBehaviour
    {
        [SerializeField] private GameObject _visual;
        [SerializeField] private VFXPlayer _deathVfxPlayer;

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
            _deathVfxPlayer.Play();
        }

        private void DeathVfxPlayer_OnVFXFinished(object sender, EventArgs e)
        {
            Debug.Log("VFX FINISHED");
            PlayerDeathPresenter.Instace.EnableDeathScreen();
        }
    }
}

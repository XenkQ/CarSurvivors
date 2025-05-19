using UnityEngine;

namespace Assets.Scripts.Player.Skills.LandmineTrap
{
    public class LandmineSkill : Skill
    {
        [SerializeField] private Landmine _landminePrefab;
        [SerializeField] private Transform _landminesParent;
        [SerializeField] private float _cooldown;
        private const float SPAWN_FIRST_LANDMINE_DELAY = 0.5f;

        private void OnEnable()
        {
            InvokeRepeating(nameof(SpawnLandmine), SPAWN_FIRST_LANDMINE_DELAY, _cooldown);
        }

        private void SpawnLandmine()
        {
            Instantiate(_landminePrefab, transform.position, Quaternion.identity, _landminesParent);
        }
    }
}

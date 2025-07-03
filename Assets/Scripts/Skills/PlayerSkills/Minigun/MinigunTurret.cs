using UnityEngine;
using Assets.Scripts.Extensions;
using Assets.ScriptableObjects;
using DG.Tweening;

namespace Assets.Scripts.Skills.PlayerSkills.Minigun
{
    public class MinigunTurret : Turret<TurretConfigSO>
    {
        [SerializeField] private bool _inverseRotation;
        private Tween _rotationTween;

        public override void Initialize(TurretConfigSO config)
        {
            base.Initialize(config);

            _visual.localRotation = Quaternion.Euler(0, (_inverseRotation ? _config.RotationAngle : -_config.RotationAngle) * 0.5f, 0);

            StartInYAngleRotation();
        }

        private void StartInYAngleRotation()
        {
            if (_rotationTween != null)
            {
                _rotationTween.Kill();
            }

            _rotationTween = _visual.StartYAngleLocalRotationLoopTween(_config.RotationAngle, _config.RotationDuration, _inverseRotation);
        }

        public override void Shoot()
        {
            Projectile projectile = Instantiate(_turretsProejctile, _gunTip.position, _gunTip.rotation, _projectilesParent);
            projectile.OnLifeEnd += Projectile_OnLifeEnd;
            projectile.Initialize(_config.ProjectileStatsSO);
        }

        private void Projectile_OnLifeEnd(object sender, System.EventArgs e)
        {
            Projectile projectile = sender as Projectile;
            if (projectile != null)
            {
                projectile.OnLifeEnd -= Projectile_OnLifeEnd;
                Destroy(projectile.gameObject);
            }
        }
    }
}

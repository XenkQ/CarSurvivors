using Assets.ScriptableObjects;
using Assets.Scripts.LayerMasks;
using Assets.Scripts.Skills;
using Assets.Scripts.StatusAffectables;
using UnityEngine;

public class LasergunTurret : Turret<TurretConfigSO>
{
    [SerializeField] private LineRenderer _laserLineRenderer;
    [SerializeField] private float _startShowLaserShootDuration = 0.1f;
    private float _currentShowLaserShootDuration;
    private bool _isShooting;
    private Collider _currentTarget;

    public override void Initialize(TurretConfigSO config)
    {
        _config = config;

        gameObject.SetActive(true);

        _currentShowLaserShootDuration = _startShowLaserShootDuration;

        InvokeRepeating(nameof(HandleTargetAssigment), 0f, _config.SearchForTargetInterval);
    }

    private void FixedUpdate()
    {
        HandleRotation();
    }

    private void Update()
    {
        if (_isShooting)
        {
            if (_currentShowLaserShootDuration <= 0)
            {
                _isShooting = false;
                _laserLineRenderer.positionCount = 0;
                _currentShowLaserShootDuration = _startShowLaserShootDuration;
            }
            else
            {
                if (_currentTarget is not null)
                {
                    _laserLineRenderer.SetPosition(0, _gunTip.position);
                    _laserLineRenderer.SetPosition(1, _currentTarget.ClosestPoint(_gunTip.position));
                    _currentShowLaserShootDuration -= Time.deltaTime;
                }
                else
                {
                    _currentShowLaserShootDuration = 0;
                }
            }
        }
    }

    public override void Shoot()
    {
        if (_currentTarget is null || _isShooting)
        {
            return;
        }

        _isShooting = true;

        _laserLineRenderer.positionCount = 2;

        if (_currentTarget.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(_config.ProjectileStatsSO.Damage);
        }
    }

    private void HandleTargetAssigment()
    {
        if (_currentTarget is not null)
        {
            if (!IsCurrentTargetInRange())
            {
                _currentTarget = null;
            }

            return;
        }

        Collider[] potentialTargets = Physics.OverlapSphere(transform.position, _config.Range, EntityLayers.Enemy);

        if (potentialTargets.Length > 0)
        {
            bool anyFound = false;
            Collider closestTarget = potentialTargets[0];
            float closestDistance = Vector3.Distance(transform.position, closestTarget.transform.position);

            foreach (Collider target in potentialTargets)
            {
                bool isBehindObstacle = Physics.Linecast(
                    _gunTip.position,
                    target.ClosestPoint(_gunTip.position),
                    TerrainLayers.All
                );

                if (isBehindObstacle)
                {
                    continue;
                }

                float currentDistance = Vector3.Distance(transform.position, target.transform.position);
                if (currentDistance <= _config.Range
                    && currentDistance <= closestDistance)
                {
                    closestDistance = currentDistance;
                    closestTarget = target;
                    anyFound = true;
                }
            }

            if (anyFound)
            {
                _currentTarget = closestTarget;
            }
        }
        else
        {
            _currentTarget = null;
        }
    }

    private bool IsCurrentTargetInRange()
    {
        return _currentTarget is not null
               && Vector3.Distance(transform.position, _currentTarget.transform.position) <= _config.Range;
    }

    private void HandleRotation()
    {
        if (_currentTarget == null)
            return;

        Vector3 targetPosition = _currentTarget.transform.position;
        Vector3 turretPosition = transform.position;
        Vector3 direction = new Vector3(
            targetPosition.x - turretPosition.x,
            0f,
            targetPosition.z - turretPosition.z
        );

        if (direction.sqrMagnitude < 0.0001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

        // Calculate rotation speed in degrees per second
        float rotationSpeed = 360f / Mathf.Max(_config.RotationDuration, 0.0001f);

        // Smoothly rotate towards the target
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.fixedDeltaTime
        );
    }
}

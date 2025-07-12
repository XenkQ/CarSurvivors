using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Car
{
    [RequireComponent(typeof(CarController))]
    public class CarVfxEffectsController : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _carMeshRenderer;
        private ICarController _carController;

        [Header("Car Stop Effect")]
        [SerializeField] private GameObject _carBackLightsHolder;
        private Material _carStopLightsMat;
        private const string CAR_STOP_LIGHTS_MAT_NAME = "CarStopLights";

        [Header("Car Fast Effect")]
        [SerializeField] private TrailRenderer[] _trailRenderers;
        [SerializeField] private float _trailDisapearingSpeed = 0.3f;
        [SerializeField] private float _tresholdToStartSpeedTrail;
        private const float SPEED_CHECK_FOR_TRAIL_DELAY = 0.1f;

        private void Awake()
        {
            _carController = GetComponent<ICarController>();
        }

        private void Start()
        {
            _carStopLightsMat = _carMeshRenderer.materials.FirstOrDefault(m => m.name == CAR_STOP_LIGHTS_MAT_NAME);

            _carController.OnBrakePress += CarController_OnBrakePress;
            _carController.OnBrakeRelease += CarController_OnBrakeRelease;

            InvokeRepeating(
                nameof(ActivateSpeedTrailSpeedExceedsTreshold),
                SPEED_CHECK_FOR_TRAIL_DELAY,
                SPEED_CHECK_FOR_TRAIL_DELAY
            );

            SetTrailsDisapearingSpeed(_trailDisapearingSpeed);

            SetTrailEmmitting(false);
        }

        private void CarController_OnBrakePress(object sender, System.EventArgs e)
        {
            _carStopLightsMat?.SetFloat("IsGlowing", 1f);
            _carBackLightsHolder.SetActive(true);
        }

        private void CarController_OnBrakeRelease(object sender, System.EventArgs e)
        {
            _carStopLightsMat?.SetFloat("IsGlowing", 0f);
            _carBackLightsHolder.SetActive(false);
        }

        private void ActivateSpeedTrailSpeedExceedsTreshold()
        {
            if (_tresholdToStartSpeedTrail <= _carController.GetMovementSpeed())
            {
                SetTrailEmmitting(true);
            }
            else
            {
                SetTrailEmmitting(false);
            }
        }

        private void SetTrailsDisapearingSpeed(float speed)
        {
            foreach (var trailRenderer in _trailRenderers)
            {
                trailRenderer.time = speed;
            }
        }

        private void SetTrailEmmitting(bool isEmitting)
        {
            foreach (var trailRenderer in _trailRenderers)
            {
                trailRenderer.emitting = isEmitting;
            }
        }
    }
}

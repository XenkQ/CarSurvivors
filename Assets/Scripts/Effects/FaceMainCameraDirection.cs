using UnityEngine;

namespace Assets.Scripts.Effects
{
    public class FaceMainCameraDirection : MonoBehaviour
    {
        private Camera _mainCamera;

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        private void FixedUpdate()
        {
            transform.LookAt(_mainCamera.transform.position);
        }
    }
}

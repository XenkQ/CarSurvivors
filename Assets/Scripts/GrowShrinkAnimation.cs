using DG.Tweening;
using System;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public struct GrowShrinkAnimationConfiguration
    {
        public float AnimationTimeToLoop;
        public float AnimationScaleMultiplier;
        public Vector3 StartScale;

        public GrowShrinkAnimationConfiguration(Vector3 startScale, float animationSpeed = 1f, float animationScaleMultiplier = 1f)
        {
            AnimationTimeToLoop = animationSpeed;
            AnimationScaleMultiplier = animationScaleMultiplier;
            StartScale = startScale;
        }
    }

    [CustomEditor(typeof(GrowShrinkAnimation))]
    public class GrowShrinkAnmiationEditor : Editor
    {
        #region SerializedProperties

        private SerializedProperty _enableHandConfiguration;
        private SerializedProperty _configuration;

        private void OnEnable()
        {
            _enableHandConfiguration = serializedObject.FindProperty(nameof(_enableHandConfiguration));
            _configuration = serializedObject.FindProperty(nameof(_configuration));
        }

        #endregion SerializedProperties

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_enableHandConfiguration);
            if (_enableHandConfiguration.boolValue)
            {
                EditorGUILayout.PropertyField(_configuration);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }

    public class GrowShrinkAnimation : MonoBehaviour
    {
        [SerializeField][Tooltip("Allows animation configuration in inspector")] private bool _enableHandConfiguration;
        [SerializeField] private GrowShrinkAnimationConfiguration _configuration;

        private Tween _scaleTween;

        private void OnEnable()
        {
            if (_enableHandConfiguration)
            {
                StartAnimation();
            }
        }

        private void OnDisable()
        {
            if (_enableHandConfiguration)
            {
                StopAnimation();
            }
        }

        public void Initialize(GrowShrinkAnimationConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void StartAnimation()
        {
            if (_scaleTween == null)
            {
                _scaleTween = transform
                    .DOScale(_configuration.StartScale * _configuration.AnimationScaleMultiplier, _configuration.AnimationTimeToLoop)
                    .SetLoops(-1, LoopType.Yoyo);
            }
            else
            {
                _scaleTween.Restart();
            }
        }

        public void StopAnimation()
        {
            transform.localScale = _configuration.StartScale;
            if (_scaleTween != null)
            {
                _scaleTween.Pause();
            }
        }
    }
}

using Assets.Scripts.Storage;
using System;
using UnityEngine;

namespace Assets.Scripts
{
    public sealed class GraphicQualityManager : MonoBehaviour
    {
        public const string GRAPHICS_QUALITY_KEY = "GraphicsQuality";

        public static GraphicQualityManager Instance { get; private set; }

        private GraphicQualityManager()
        { }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            LoadQualitySettings();
        }

        public void LoadQualitySettings()
        {
            int savedLevel = Array.IndexOf(QualitySettings.names, AppStorage.Get<string>(GRAPHICS_QUALITY_KEY));
            SetQualityLevel(savedLevel);
        }

        public void SetQualityLevel(int level)
        {
            if (level < 0 || level >= QualitySettings.names.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(level), "Invalid quality level.");
            }

            QualitySettings.SetQualityLevel(level, true);
        }
    }
}
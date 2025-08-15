using System;
using Reflex.Attributes;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class MenuButtonsFunctionality : MonoBehaviour
    {
        [Inject] private readonly IGameSceneLoader _gameSceneLoader;

        [SerializeField] private GameObject[] _enabledDisabledPanels;

        public void OnSceneLoadButtonClick(string scene)
        {
            if (Enum.TryParse(scene, true, out GameScene gameScene))
            {
                _gameSceneLoader.LoadNewSceneAsync(gameScene);
            }
            else
            {
                Debug.LogError($"Invalid scene name: {scene}");
            }
        }

        public void EnableSingleObjectDisableOthers(GameObject gameObject)
        {
            foreach (var panel in _enabledDisabledPanels)
            {
                if (panel != gameObject)
                {
                    panel.SetActive(false);
                }
            }

            gameObject.SetActive(true);
        }

        public void ToogleActivityOfObject(GameObject gameObject)
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }

        public void OnTryAgainClick()
        {
            _gameSceneLoader.ReloadCurrentSceneAsync();
        }

        public void OnExitClick()
        {
            Application.Quit();
        }
    }
}

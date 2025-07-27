using System;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class MenuButtonsFunctionality : MonoBehaviour
    {
        public void OnSceneLoadButtonClick(string scene)
        {
            if (Enum.TryParse(scene, true, out GameScene gameScene))
            {
                GameSceneLoader.Load(gameScene);
                GameTime.ResumeTime();
            }
            else
            {
                Debug.LogError($"Invalid scene name: {scene}");
            }
        }

        public void ToogleActivityOfObject(GameObject gameObject)
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }

        public void OnTryAgainClick()
        {
            GameSceneLoader.Load(GameScene.RuinedBloodCity);
            GameTime.ResumeTime();
        }

        public void OnExitClick()
        {
            Application.Quit();
        }
    }
}

using System;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    [Serializable]
    public enum GameScene
    {
        MainMenu = 0,
        RuinedBloodCity = 1
    }

    public static class GameSceneLoader
    {
        public static GameScene CurrentGameScene => SceneManager.GetActiveScene().buildIndex switch
        {
            0 => GameScene.MainMenu,
            1 => GameScene.RuinedBloodCity,
            _ => throw new ArgumentOutOfRangeException("Unknown scene index")
        };

        public static void Load(GameScene scene)
        {
            SceneManager.LoadScene((int)scene);
            GameTime.ResumeTime();
        }

        public static void ReloadCurrentScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            GameTime.ResumeTime();
        }
    }
}
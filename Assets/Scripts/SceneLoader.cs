using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public enum Scenes
    {
        DeathCity = 0,
        MainMenu = 1
    }

    public static class SceneLoader
    {
        public static void Load(Scenes scene)
        {
            SceneManager.LoadScene((int)scene);
        }
    }
}

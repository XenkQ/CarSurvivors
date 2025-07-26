using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public enum Scenes
    {
        MainMenu = 0,
        DeathCity = 1
    }

    public static class SceneLoader
    {
        public static Scenes CurrentScene => SceneManager.GetActiveScene().buildIndex switch
        {
            0 => Scenes.MainMenu,
            1 => Scenes.DeathCity,
            _ => throw new System.ArgumentOutOfRangeException("Unknown scene index")
        };

        public static void Load(Scenes scene)
        {
            SceneManager.LoadScene((int)scene);
        }
    }
}

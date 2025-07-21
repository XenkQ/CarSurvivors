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
        public static Scenes CurrentScene => SceneManager.GetActiveScene().buildIndex switch
        {
            0 => Scenes.DeathCity,
            1 => Scenes.MainMenu,
            _ => throw new System.ArgumentOutOfRangeException("Unknown scene index")
        };

        public static void Load(Scenes scene)
        {
            SceneManager.LoadScene((int)scene);
        }
    }
}

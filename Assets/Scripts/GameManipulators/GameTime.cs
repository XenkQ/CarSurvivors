using UnityEngine;

namespace Assets.Scripts.GameManipulators
{
    public static class GameTime
    {
        public static void Pause()
        {
            Time.timeScale = 0f;
        }

        public static void Resume()
        {
            Time.timeScale = 1f;
        }
    }
}

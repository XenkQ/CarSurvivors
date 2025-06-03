using UnityEngine;

namespace Assets.Scripts
{
    public static class GameTime
    {
        public static void PauseTime()
        {
            Time.timeScale = 0f;
        }

        public static void ResumeTime()
        {
            Time.timeScale = 1f;
        }
    }
}

using UnityEngine;

namespace Assets.Scripts.UI
{
    public class MenuButtonsFunctionality : MonoBehaviour
    {
        public void OnMainMenuClick()
        {
            SceneLoader.Load(Scenes.MainMenu);
            GameTime.ResumeTime();
        }

        public void OnTryAgainClick()
        {
            SceneLoader.Load(Scenes.DeathCity);
            GameTime.ResumeTime();
        }

        public void OnExitClick()
        {
            Application.Quit();
        }
    }
}

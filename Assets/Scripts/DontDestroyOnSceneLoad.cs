using UnityEngine;

namespace Assets.Scripts
{
    public class DontDestroyOnSceneLoad : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}

using UnityEngine;

namespace Assets.Scripts.ObjectLifeCycle
{
    public class DontDestroyOnSceneLoad : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}

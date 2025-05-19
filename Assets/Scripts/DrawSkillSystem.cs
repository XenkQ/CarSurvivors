using UnityEngine;

namespace Assets.Scripts
{
    public sealed class DrawSkillSystem : MonoBehaviour
    {
        public DrawSkillSystem Instance { get; set; }

        private DrawSkillSystem()
        { }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}

using Assets.ScriptableObjects.Skills;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.UI.Skills
{
    public sealed class SkillsVisualPresenter : MonoBehaviour
    {
        [SerializeField] private GameObject[] _skillsVisuals;
        public static SkillsVisualPresenter Instance { get; private set; }

        private SkillsVisualPresenter()
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

        public void ShowSkillVisualBasedOnSkillInfo(SkillInfoSO skillInfoSO)
        {
            _skillsVisuals.First(s => s.name == skillInfoSO.Name)?.SetActive(true);
        }

        public void HideAll()
        {
            foreach (GameObject skillVisual in _skillsVisuals)
            {
                skillVisual.SetActive(false);
            }
        }
    }
}

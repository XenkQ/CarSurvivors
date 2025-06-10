using Assets.ScriptableObjects.Skills;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.UI.Skills
{
    public class SkillsVisualPresenter : MonoBehaviour
    {
        [SerializeField] private GameObject[] _skillsVisuals;

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

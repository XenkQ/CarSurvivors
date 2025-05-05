using System.Linq;
using Assets.Scripts.Player;
using UnityEngine;

public class SkillsManager : MonoBehaviour
{
    [SerializeField] private Skill[] _skills;

    private void ActivateRandomDisabledSkill()
    {
        ISkill[] inactiveSkills = _skills.Where(skill => !skill.gameObject.activeSelf).ToArray();
        if (inactiveSkills.Length > 0)
        {
            int index = Random.Range(0, inactiveSkills.Length);
            inactiveSkills[index].Activate();
        }
    }
}
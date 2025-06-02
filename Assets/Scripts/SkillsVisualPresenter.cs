using System.Linq;
using UnityEngine;

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

    public void HideSkillVisualBasedOnSkillInfo(SkillInfoSO skillInfoSO)
    {
        _skillsVisuals.First(s => s.name == skillInfoSO.Name)?.SetActive(false);
    }
}

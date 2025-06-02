using Assets.ScriptableObjects.Player.Skills;
using Assets.Scripts.Skills;
using Assets.Scripts.Skills.ObjectsImpactingSkills.Crate;
using Player;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Skills
{
    public class SkillUpgradePresenter : MonoBehaviour
    {
        [Header("Upgrade Skill")]
        [SerializeField] private GameObject _upgradeSkillSection;
        [SerializeField] private Button _upgradeButtonPrefab;
        [SerializeField] private Transform _buttonsHolder;
        private byte MAX_SKILLS_UPGRADE_BUTTONS = 3;

        [Header("New Skill")]
        [SerializeField] private GameObject _newSkillSection;
        [SerializeField] private TextMeshProUGUI _newSkillName;
        [SerializeField] private TextMeshProUGUI _newSkillDescription;
        private const string SKILL_NAME_TEMPLATE = "New Skill: {0}";

        private void Start()
        {
            CollectibleItemsSpawner.Instance.OnSpawnedEntityReleased += ShowButtonsWithRandomConfigStatsUpgrades_OnEvent;
            PlayerManager.Instance.LevelController.OnLvlUp += ShowButtonsWithRandomConfigStatsUpgrades_OnEvent;
        }

        private void ShowButtonsWithRandomConfigStatsUpgrades_OnEvent(object sender, System.EventArgs e)
        {
            GameTime.StopTime();

            ISkillBase skillToWorkWith;
            if (SkillsRegistry.Instance.UninitializedSkillsCounter > 0)
            {
                skillToWorkWith = RandomDisabledSkillsInitializer.InitializeRandomUninitializedSkill();
                ShowNewSkillPanel(skillToWorkWith);
            }
            else
            {
                skillToWorkWith = RandomUpgradeableSkillFinder.Find();
                ShowButtonsWithStatsUpgrades((skillToWorkWith as IUpgradeableSkill).Config);
            }

            SkillsVisualPresenter.Instance.ShowSkillVisualBasedOnSkillInfo(skillToWorkWith.SkillInfo);
        }

        private void ShowNewSkillPanel(ISkillBase skillBase)
        {
            _newSkillSection.SetActive(true);
            _newSkillName.text = string.Format(SKILL_NAME_TEMPLATE, skillBase.SkillInfo.Name);
            _newSkillDescription.text = skillBase.SkillInfo.Description;
        }

        private void ShowButtonsWithStatsUpgrades(ISkillUpgradeableStatsConfig skillUpgradeableStatsConfig)
        {
            List<ClickableButtonData> skillStatsUpgradeButtonsData = new List<ClickableButtonData>();

            foreach (var upgradeableStat in skillUpgradeableStatsConfig.GetUpgradeableStats())
            {
                float upgradeValue = upgradeableStat.GetUpgradeValueBasedOnUpdateRange();
                skillStatsUpgradeButtonsData.Add(new ClickableButtonData
                {
                    Text = (upgradeValue > 0 ? "Increase" : "Decrease") + $" {upgradeableStat.GetType().Name} by {upgradeValue}",
                    OnClick = () =>
                    {
                        upgradeableStat.Upgrade(upgradeValue);
                        _upgradeSkillSection.SetActive(false);
                        GameTime.ResumeTime();
                    }
                });
            }

            DisplayNewButtons(skillStatsUpgradeButtonsData.Shuffle().Take(MAX_SKILLS_UPGRADE_BUTTONS));

            _upgradeSkillSection.SetActive(true);
        }

        private void DisplayNewButtons(IEnumerable<ClickableButtonData> clickableButtonsData)
        {
            DestroyAllButtons();
            CreateUpgradeButtons(clickableButtonsData);
        }

        private void CreateUpgradeButtons(IEnumerable<ClickableButtonData> clickableButtonsData)
        {
            foreach (var clickableButtonData in clickableButtonsData)
            {
                Button button = Instantiate(_upgradeButtonPrefab, _buttonsHolder);
                button.onClick.AddListener(() => clickableButtonData.OnClick?.Invoke());

                if (button.TryGetComponent(out TextMeshProUGUI buttonText))
                {
                    buttonText.text = clickableButtonData.Text;
                }
            }
        }

        private void DestroyAllButtons()
        {
            foreach (Transform child in _buttonsHolder)
            {
                if (child.gameObject.TryGetComponent<Button>(out _))
                {
                    Destroy(child.gameObject);
                }
            }
        }
    }
}

﻿using Assets.Scripts.Extensions;
using Assets.Scripts.Skills;
using Assets.Scripts.Skills.ObjectsImpactingSkills.Crate;
using Assets.Scripts.UI.Level;
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
        [SerializeField] private Button _continueButton;
        private const string SKILL_NAME_TEMPLATE = "New Skill: {0}";

        private Queue<ISkillBase> _skillsQueuedForInitialization = new Queue<ISkillBase>();
        private Queue<IUpgradeableSkill> _skillsQueuedForUpgrade = new Queue<IUpgradeableSkill>();

        private SkillsVisualPresenter _skillsVisualPresenter;

        private bool _isShowingAnySection;

        private void Start()
        {
            _skillsVisualPresenter = GameObject
                .FindGameObjectWithTag(typeof(SkillsVisualPresenter).Name)
                .GetComponent<SkillsVisualPresenter>();

            CollectibleItemsSpawner.Instance.OnSpawnedEntityReleased += ShowRandomSkillInInitializationOrUpgradeSection_OnEvent;
            PlayerLevelPresenter.Instance.OnExpSliderVisualEndValueReached += ShowRandomSkillInInitializationOrUpgradeSection_OnEvent;
            _continueButton.onClick.AddListener(() => HandleUpgradeableOrInitializableSkillsShowing());
        }

        private void ShowRandomSkillInInitializationOrUpgradeSection_OnEvent(object sender, System.EventArgs e)
        {
            if (_skillsQueuedForInitialization.Count < SkillsRegistry.Instance.UninitializedSkillsCount)
            {
                _skillsQueuedForInitialization.Enqueue(RandomUninitializedSkillsInitializator.Initialize());
            }
            else
            {
                _skillsQueuedForUpgrade.Enqueue(RandomUpgradeableSkillFinder.Find());
            }

            if (!_isShowingAnySection)
            {
                _isShowingAnySection = true;
                HandleUpgradeableOrInitializableSkillsShowing();
            }
        }

        private void HandleUpgradeableOrInitializableSkillsShowing()
        {
            _skillsVisualPresenter.HideAll();

            GameTime.ResumeTime();

            if (_skillsQueuedForInitialization.Count > 0)
            {
                ISkillBase skill = _skillsQueuedForInitialization.Dequeue();
                SkillsRegistry.Instance.InitializeSkill(skill);
                ShowNewSkillSection(skill);
                GameTime.PauseTime();
            }
            else if (_skillsQueuedForUpgrade.Count > 0)
            {
                IUpgradeableSkill skill = _skillsQueuedForUpgrade.Dequeue();
                ShowStatsUpgradeSection(skill);
                GameTime.PauseTime();
            }
            else
            {
                _newSkillSection.SetActive(false);
                _upgradeSkillSection.SetActive(false);
                _isShowingAnySection = false;
            }
        }

        private void ShowNewSkillSection(ISkillBase skillBase)
        {
            _newSkillName.text = string.Format(SKILL_NAME_TEMPLATE, skillBase.SkillInfo.Name);
            _newSkillDescription.text = skillBase.SkillInfo.Description;

            _skillsVisualPresenter.ShowSkillVisualBasedOnSkillInfo(skillBase.SkillInfo);

            _newSkillSection.SetActive(true);
        }

        private void ShowStatsUpgradeSection(IUpgradeableSkill upgradeableStats)
        {
            List<ClickableButtonData> skillStatsUpgradeButtonsData = new List<ClickableButtonData>();

            foreach (var nameUpgradeableStatPair in upgradeableStats.Config.GetUpgradeableStatsThatCanBeUpgraded())
            {
                float upgradeValue = nameUpgradeableStatPair.UpgradeableStat.GetUpgradeValueBasedOnUpdateRange();
                IUpgradeableStat upgradeableStat = nameUpgradeableStatPair.UpgradeableStat;

                skillStatsUpgradeButtonsData.Add(new ClickableButtonData
                {
                    Text = (upgradeableStat.IsSubstractModeOn ? "Decrease" : "Increase")
                        + $" {nameUpgradeableStatPair.Name.PascalCaseToWords()} by "
                        + (upgradeableStat.Unit == StatsUnits.Percentage ?
                            upgradeableStat.GetWhatPercentOfValueIsUpgradeValue(upgradeValue) : upgradeValue)
                        + upgradeableStat.Unit.ToDisplayString(),

                    OnClick = () =>
                    {
                        nameUpgradeableStatPair.UpgradeableStat.Upgrade(upgradeValue);
                        HandleUpgradeableOrInitializableSkillsShowing();
                    }
                });
            }

            DisplayNewButtons(skillStatsUpgradeButtonsData.Shuffle().Take(MAX_SKILLS_UPGRADE_BUTTONS));

            _skillsVisualPresenter.ShowSkillVisualBasedOnSkillInfo(upgradeableStats.SkillInfo);

            _upgradeSkillSection.SetActive(true);
        }

        private void DisplayNewButtons(IEnumerable<ClickableButtonData> clickableButtonsData)
        {
            DestroyAllButtons();
            CreateUpgradeButtons(clickableButtonsData);
        }

        private void DestroyAllButtons()
        {
            foreach (Button child in _buttonsHolder.GetComponentsInChildren<Button>())
            {
                Destroy(child.gameObject);
            }
        }

        private void CreateUpgradeButtons(IEnumerable<ClickableButtonData> clickableButtonsData)
        {
            foreach (var clickableButtonData in clickableButtonsData)
            {
                Button button = Instantiate(_upgradeButtonPrefab, _buttonsHolder);
                button.onClick.AddListener(() => clickableButtonData.OnClick?.Invoke());
                TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();

                if (button != null)
                {
                    buttonText.text = clickableButtonData.Text;
                }
            }
        }
    }
}

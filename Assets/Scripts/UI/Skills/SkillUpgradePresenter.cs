using Assets.ScriptableObjects.Player.Skills;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Skills
{
    public class SkillUpgradePresenter : MonoBehaviour
    {
        private byte MAX_SKILLS_UPGRADE_BUTTONS = 3;

        [SerializeField] private GameObject _upgradePanel;
        [SerializeField] private Button _buttonPrefab;
        [SerializeField] private Transform _buttonsHolder;

        public void ShowButtonsWithStatsUpgrades(ISkillUpgradeableStatsConfig skillUpgradeableStatsConfig)
        {
            List<ClickableButtonData> skillStatsUpgradeButtonsData = new List<ClickableButtonData>();

            foreach (var upgradeableStat in skillUpgradeableStatsConfig.GetUpgradeableStats())
            {
                float upgradeValue = upgradeableStat.GetUpgradeValueBasedOnUpdateRange();
                skillStatsUpgradeButtonsData.Add(new ClickableButtonData
                {
                    Text = (upgradeValue > 0 ? "Increase" : "Decrease") + $" {upgradeableStat.GetType().Name} by {upgradeValue}",
                    OnClick = () => upgradeableStat.Upgrade(upgradeValue)
                });
            }

            DisplayNewButtons(skillStatsUpgradeButtonsData.Shuffle().Take(MAX_SKILLS_UPGRADE_BUTTONS));

            _upgradePanel.SetActive(true);
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
                Button button = Instantiate(_buttonPrefab, _buttonsHolder);
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

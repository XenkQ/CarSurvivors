using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Skills
{
    public class SkillUpgraderUI : MonoBehaviour
    {
        [SerializeField] private Button _buttonPrefab;
        [SerializeField] private Transform _buttonsHolder;

        public void SetUpgradeButtons(IEnumerable<string> buttonsTexts, Action onClick)
        {
            DestroyAllButtons();
            CreateUpgradeButtons(buttonsTexts, onClick);
        }

        private void CreateUpgradeButtons(IEnumerable<string> buttonsTexts, Action onClick)
        {
            foreach (string text in buttonsTexts)
            {
                Button button = Instantiate(_buttonPrefab, _buttonsHolder);
                button.onClick.AddListener(() => onClick?.Invoke());

                if (button.TryGetComponent(out TextMeshProUGUI buttonText))
                {
                    buttonText.text = text;
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

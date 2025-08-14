using Assets.Scripts.Utils;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.ScoreBoard
{
    public class ScoreBoardEntry : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _orderNumberText;
        [SerializeField] private TextMeshProUGUI _scoreText;

        public void SetOrderNumber(byte number)
        {
            _orderNumberText.text = number.ToString();
        }

        public void SetScore(uint score)
        {
            _scoreText.text = TimeConversionUtility.FormatSecondsToTimeString(score);
        }
    }
}

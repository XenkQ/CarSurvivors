using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public sealed class TimerPresenter : MonoBehaviour
    {
        public static TimerPresenter Instance { get; private set; }
        [SerializeField] private TextMeshProUGUI _timerText;
        public uint TimerValue { get; private set; }

        private TimerPresenter()
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

        private void Start()
        {
            InvokeRepeating(nameof(IncreaseTimer), 1f, 1f);
        }

        private void IncreaseTimer()
        {
            TimerValue++;
            _timerText.text = TimerValue.ToString();
        }
    }
}

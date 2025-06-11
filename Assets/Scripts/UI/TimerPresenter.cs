using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
public class TimerPresenter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerText;
    private uint _timerValue;

    private void Start()
    {
        InvokeRepeating(nameof(IncreaseTimer), 1f, 1f);
    }

    private void IncreaseTimer()
    {
        _timerValue++;
        _timerText.text = _timerValue.ToString();
    }
}
}
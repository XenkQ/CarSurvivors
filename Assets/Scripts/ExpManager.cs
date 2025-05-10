using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class ExpManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private Slider _expSlider;
    [SerializeField] private AnimationCurve _expCurve;
    [SerializeField] private float _timeToVisualiseCurrentExpInLvlOnSlider = 4f;
    private float _currentExpInLvl;
    private float _expToNextLvl;
    private byte _currentLevel = 1;

    public ExpManager Instance { get; private set; }

    private Tween _expIncreaseTween;

    private ExpManager()
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
        _expToNextLvl = CalculateExpToNextLevel(_currentLevel);
        _expSlider.maxValue = _expToNextLvl;
        _expSlider.value = _currentExpInLvl;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            AddExp(20f);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            AddExp(1000000f);
        }
    }

    public void AddExp(float exp)
    {
        if (_currentLevel == byte.MaxValue)
        {
            return;
        }

        _currentExpInLvl += exp;

        while (_currentLevel < byte.MaxValue && _currentExpInLvl >= _expToNextLvl)
        {
            _currentExpInLvl -= _expToNextLvl;
            _currentLevel++;
            _expToNextLvl = CalculateExpToNextLevel(_currentLevel);
        }

        UpdateExpVisuals();
    }

    private float CalculateExpToNextLevel(byte level)
        => _expCurve.Evaluate(level);

    private void UpdateExpVisuals()
    {
        if (_expIncreaseTween != null)
        {
            _expIncreaseTween.Kill();
        }

        if (_expSlider.maxValue < _expToNextLvl)
        {
            _expIncreaseTween = TweenSliderToValue(_expSlider.maxValue)
                .OnComplete(() =>
                {
                    UpdateLvlText(_currentLevel);
                    _expSlider.value = 0;
                    _expIncreaseTween = TweenSliderToValue(_currentExpInLvl);
                });
        }
        else
        {
            _expIncreaseTween = TweenSliderToValue(_currentExpInLvl);
        }

        _expSlider.maxValue = _expToNextLvl;
    }

    private void UpdateLvlText(byte level)
    => _levelText.text = $"{_currentLevel} Lvl";

    private Tween TweenSliderToValue(float value)
    {
        return DOTween
            .To(
                () => _expSlider.value,
                x => _expSlider.value = x,
                value,
                _timeToVisualiseCurrentExpInLvlOnSlider)
            .SetEase(Ease.InOutSine);
    }
}

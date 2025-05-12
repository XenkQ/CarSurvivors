using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class ExpSliderBar : MonoBehaviour
    {
        private const float FASTEST_EXP_INCREASE_ANIM_SPEED = 0.5f;
        private const float SLOWEST_EXP_INCREASE_ANIM_SPEED = 4f;
        private const float EXP_PERCENT_ANIM_SPEED_BOOST_BY_LVL_DIFF = 0.1f;

        private const float START_CHECKING_FOR_LEVEL_UP_DELAY = 0.6f;
        private const float DELAY_BETWEEN_LEVEL_UP_CHECK = 0.1f;

        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private Slider _expSlider;

        private ExpData _expData;
        private Queue<ExpData> _levelUpQueue = new();

        private Tween _expIncreaseTween;
        private Tween _lvlUpTween;

        private void Start()
        {
            _expData = ExpManager.Instance.ExpData;
            _expSlider.value = _expData.Exp;
            _expSlider.maxValue = _expData.MaxExp;

            ExpManager.Instance.OnExpChange += ExpManager_OnExpChange;
            ExpManager.Instance.OnLvlUp += ExpManager_OnLvlChange;

            InvokeRepeating(nameof(HandleLevelUpQueue), START_CHECKING_FOR_LEVEL_UP_DELAY, DELAY_BETWEEN_LEVEL_UP_CHECK);
        }

        private void ExpManager_OnExpChange(object sender, ExpDataEventArgs e)
        {
            bool notNextLvl = e.ExpData.Exp < _expData.MaxExp;
            bool currentlyNotPlayingLvlUpAnim = _lvlUpTween == null
                                                || (_levelUpQueue.Count == 0 && !_lvlUpTween.IsPlaying());

            if (notNextLvl && currentlyNotPlayingLvlUpAnim)
            {
                KillExpIncreaseTweenIfPlaying();

                _expData = new ExpData(_expData.Lvl, e.ExpData.Exp, _expData.MaxExp);
                _expIncreaseTween = AnimateSliderExpGain(_expData.Exp);
            }
        }

        private void ExpManager_OnLvlChange(object sender, ExpDataEventArgs e)
        {
            _levelUpQueue.Enqueue(e.ExpData);
        }

        private void HandleLevelUpQueue()
        {
            if (_levelUpQueue.Count > 0 && (_lvlUpTween == null || !_lvlUpTween.IsPlaying()))
            {
                KillExpIncreaseTweenIfPlaying();

                _lvlUpTween = AnimateSliderExpGain(_expData.MaxExp).OnComplete(() =>
                {
                    _expData = _levelUpQueue.Dequeue();
                    _expSlider.value = 0;
                    _expSlider.maxValue = _expData.MaxExp;

                    UpdateLvlText();

                    if (_levelUpQueue.Count == 0)
                    {
                        _expIncreaseTween = AnimateSliderExpGain(_expData.Exp);
                    }
                });
            }
        }

        private void KillExpIncreaseTweenIfPlaying()
        {
            if (_expIncreaseTween != null && _expIncreaseTween.IsPlaying())
            {
                _expIncreaseTween.Kill();
            }
        }

        private Tween AnimateSliderExpGain(float exp)
        {
            return _expSlider.DOValue(exp, CalculateSliderExpGainAnimSpeed(exp))
                                .SetEase(Ease.OutQuad);
        }

        private float CalculateSliderExpGainAnimSpeed(float newExp)
        {
            byte levelsDiff = (byte)(ExpManager.Instance.ExpData.Lvl - _expData.Lvl);
            float speedBoost = levelsDiff * EXP_PERCENT_ANIM_SPEED_BOOST_BY_LVL_DIFF;

            float slowestSpeedPercent;
            if (newExp > 0)
            {
                slowestSpeedPercent = 1f - (_expSlider.value / newExp);
            }
            else
            {
                slowestSpeedPercent = 1f;
            }

            slowestSpeedPercent = Mathf.Clamp01(slowestSpeedPercent - speedBoost);

            return Mathf.Max(FASTEST_EXP_INCREASE_ANIM_SPEED, SLOWEST_EXP_INCREASE_ANIM_SPEED * slowestSpeedPercent);
        }

        private void UpdateLvlText()
            => _levelText.text = $"{_expData.Lvl} Lvl";
    }
}

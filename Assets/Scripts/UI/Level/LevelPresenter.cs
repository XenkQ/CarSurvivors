using Assets.Scripts.Exp;
using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Level
{
    public class LevelPresenter : MonoBehaviour
    {
        private const float FASTEST_EXP_INCREASE_ANIM_SPEED = 0.5f;
        private const float SLOWEST_EXP_INCREASE_ANIM_SPEED = 4f;
        private const float EXP_PERCENT_ANIM_SPEED_BOOST_BY_LVL_DIFF = 0.1f;

        private const float DELAY_BETWEEN_TWEENS_ANIMATION_CHECK = 0.05f;

        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private Slider _expSlider;

        private LevelData _expData;
        private Queue<LevelData> _levelUpQueue = new();
        private LevelData? _currentExpIncreaseData;

        private Tween _expIncreaseTween;
        private Tween _lvlUpTween;

        private void Start()
        {
            _expData = LevelSystem.Instance.ExpData;
            _expSlider.value = _expData.Exp;
            _expSlider.maxValue = _expData.MaxExp;

            LevelSystem.Instance.OnExpChange += ExpManager_OnExpChange;
            LevelSystem.Instance.OnLvlUp += ExpManager_OnLvlChange;

            InvokeRepeating(nameof(HandleTweensAnimations), DELAY_BETWEEN_TWEENS_ANIMATION_CHECK, DELAY_BETWEEN_TWEENS_ANIMATION_CHECK);
        }

        private void ExpManager_OnExpChange(object sender, ExpDataEventArgs e)
        {
            _currentExpIncreaseData = e.ExpData;
        }

        private void ExpManager_OnLvlChange(object sender, ExpDataEventArgs e)
        {
            _levelUpQueue.Enqueue(e.ExpData);
        }

        private void HandleTweensAnimations()
        {
            if (_levelUpQueue.Count > 0 && !IsPlayingLvlUpTween())
            {
                KillExpIncreaseTweenIfPlaying();
                PlayLvlUpTween();
            }
            else if (CanPlayLastExpIncreaseTween())
            {
                KillExpIncreaseTweenIfPlaying();
                PlayLastExpIncreaseTween();
            }
        }

        private bool IsPlayingLvlUpTween()
        {
            return _lvlUpTween != null && _lvlUpTween.IsPlaying();
        }

        private void KillExpIncreaseTweenIfPlaying()
        {
            if (_expIncreaseTween != null && _expIncreaseTween.IsPlaying())
            {
                _expIncreaseTween.Kill();
            }
        }

        private void PlayLvlUpTween()
        {
            _lvlUpTween = AnimateSliderExpGain(_expData.MaxExp).OnComplete(() =>
            {
                _expData = _levelUpQueue.Dequeue();
                _expSlider.value = 0;
                _expSlider.maxValue = _expData.MaxExp;

                UpdateLvlText();

                if (_levelUpQueue.Count == 0)
                {
                    if (_currentExpIncreaseData.Value.Exp > _expData.Exp && CanPlayLastExpIncreaseTween())
                    {
                        KillExpIncreaseTweenIfPlaying();
                        PlayLastExpIncreaseTween();
                    }
                    else
                    {
                        KillExpIncreaseTweenIfPlaying();
                        _expIncreaseTween = AnimateSliderExpGain(_expData.Exp);
                    }
                }
            });
        }

        private void PlayLastExpIncreaseTween()
        {
            _expIncreaseTween = AnimateSliderExpGain(_currentExpIncreaseData.Value.Exp);
        }

        private bool CanPlayLastExpIncreaseTween()
        {
            return _currentExpIncreaseData != null
                && !IsPlayingLvlUpTween()
                && _currentExpIncreaseData.Value.Lvl == _expData.Lvl;
        }

        private Tween AnimateSliderExpGain(float exp)
        {
            return _expSlider.DOValue(exp, CalculateSliderExpGainAnimSpeed(exp))
                                .SetEase(Ease.OutQuad);
        }

        private float CalculateSliderExpGainAnimSpeed(float newExp)
        {
            byte levelsDiff = (byte)(LevelSystem.Instance.ExpData.Lvl - _expData.Lvl);
            float speedBoost = levelsDiff * EXP_PERCENT_ANIM_SPEED_BOOST_BY_LVL_DIFF;

            float slowestSpeedPercent;
            if (newExp > 0)
            {
                slowestSpeedPercent = 1f - _expSlider.value / newExp;
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

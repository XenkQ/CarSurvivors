using Assets.Scripts.LevelSystem;
using DG.Tweening;
using Assets.Scripts.Player;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Level
{
    public sealed class PlayerLevelPresenter : MonoBehaviour
    {
        private const float FASTEST_EXP_INCREASE_ANIM_SPEED = 0.5f;
        private const float SLOWEST_EXP_INCREASE_ANIM_SPEED = 4f;
        private const float EXP_PERCENT_ANIM_SPEED_BOOST_BY_LVL_DIFF = 0.1f;

        private const float DELAY_BETWEEN_TWEENS_ANIMATION_CHECK = 0.05f;

        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private Slider _expSlider;

        public event EventHandler OnExpSliderVisualEndValueReached;

        public static PlayerLevelPresenter Instance { get; private set; }

        private ILevelController _playerLevelController;

        private LevelData _currentlyVisibleLevelData;
        private LevelData _newestLevelData;
        private readonly Queue<LevelData> _levelDataToShowQueue = new();

        private Tween _expIncreaseTween;
        private Tween _levelUpTween;

        private PlayerLevelPresenter()
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
            _playerLevelController = PlayerManager.Instance.LevelController;

            _currentlyVisibleLevelData = _playerLevelController.LevelData;
            _expSlider.value = _currentlyVisibleLevelData.Exp;
            _expSlider.maxValue = _currentlyVisibleLevelData.MaxExp;

            _playerLevelController.OnExpChange += LevelController_OnExpChange;
            _playerLevelController.OnLvlUp += LevelController_OnLvlChange;

            InvokeRepeating(nameof(HandleTweensAnimations), 0, DELAY_BETWEEN_TWEENS_ANIMATION_CHECK);
        }

        private void LevelController_OnExpChange(object sender, LevelDataEventArgs e)
        {
            _newestLevelData = e.ExpData;
        }

        private void LevelController_OnLvlChange(object sender, LevelDataEventArgs e)
        {
            _levelDataToShowQueue.Enqueue(e.ExpData);
        }

        private void HandleTweensAnimations()
        {
            if (_levelDataToShowQueue.Count > 0 && !IsPlayingLevelUpTween())
            {
                KillExpIncreaseTweenIfPlaying();
                PlayLevelUpTween();
            }
            else if (CanPlayLastExpIncreaseTween())
            {
                KillExpIncreaseTweenIfPlaying();
                PlayLastExpIncreaseTween();
            }
        }

        private bool IsPlayingLevelUpTween()
        {
            return _levelUpTween?.IsPlaying() == true;
        }

        private void KillExpIncreaseTweenIfPlaying()
        {
            if (_expIncreaseTween != null && _expIncreaseTween.IsPlaying())
            {
                _expIncreaseTween.Kill();
                _expIncreaseTween = null;
            }
        }

        private void PlayLevelUpTween()
        {
            _levelUpTween = AnimateSliderExpGain(_currentlyVisibleLevelData.MaxExp).OnComplete(() =>
            {
                _currentlyVisibleLevelData = _levelDataToShowQueue.Dequeue();
                _expSlider.value = 0;
                _expSlider.maxValue = _currentlyVisibleLevelData.MaxExp;

                UpdateLevelText();

                OnExpSliderVisualEndValueReached?.Invoke(this, EventArgs.Empty);

                if (_levelDataToShowQueue.Count == 0)
                {
                    if (_newestLevelData.Exp > _currentlyVisibleLevelData.Exp && CanPlayLastExpIncreaseTween())
                    {
                        KillExpIncreaseTweenIfPlaying();
                        PlayLastExpIncreaseTween();
                    }
                    else
                    {
                        KillExpIncreaseTweenIfPlaying();
                        _expIncreaseTween = AnimateSliderExpGain(_currentlyVisibleLevelData.Exp);
                    }
                }
            });
        }

        private void PlayLastExpIncreaseTween()
        {
            _expIncreaseTween = AnimateSliderExpGain(_newestLevelData.Exp);
        }

        private bool CanPlayLastExpIncreaseTween()
        {
            return !IsPlayingLevelUpTween()
                && _newestLevelData.Lvl == _currentlyVisibleLevelData.Lvl;
        }

        private Tween AnimateSliderExpGain(float exp)
        {
            return _expSlider.DOValue(exp, CalculateSliderExpGainAnimSpeed(exp))
                                .SetEase(Ease.OutQuad);
        }

        private float CalculateSliderExpGainAnimSpeed(float newExp)
        {
            byte levelsDiff = (byte)(_playerLevelController.LevelData.Lvl - _currentlyVisibleLevelData.Lvl);
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

        private void UpdateLevelText()
            => _levelText.text = $"{_currentlyVisibleLevelData.Lvl} Lvl";
    }
}

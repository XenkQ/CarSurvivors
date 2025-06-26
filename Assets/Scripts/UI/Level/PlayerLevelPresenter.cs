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
        private const float BASE_EXP_INCREASE_ANIM_SPEED = 1f;
        private const float FASTEST_EXP_INCREASE_ANIM_SPEED = 0.6f;
        private const float DELAY_BETWEEN_TWEENS_ANIMATION_CHECK = 0.02f;

        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private Slider _expSlider;

        public event EventHandler OnExpSliderVisualEndValueReached;

        public static PlayerLevelPresenter Instance { get; private set; }

        private ILevelController _playerLevelController;

        private LevelData _currentlyVisibleLevelData;
        private readonly Queue<ExpVisualEvent> _expVisualQueue = new();
        private ExpVisualEvent? _lastQueuedExpInSameLevelIncreaseEvent = null;

        private Tween _expIncreaseTween;
        private Tween _levelUpTween;

        private struct ExpVisualEvent
        {
            public LevelData LevelData;
            public bool IsLevelUp;

            public ExpVisualEvent(LevelData data, bool isLevelUp)
            {
                LevelData = data;
                IsLevelUp = isLevelUp;
            }
        }

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
            if (e.ExpData.Lvl == _currentlyVisibleLevelData.Lvl)
            {
                _lastQueuedExpInSameLevelIncreaseEvent = new ExpVisualEvent(e.ExpData, false);
            }
        }

        private void LevelController_OnLvlChange(object sender, LevelDataEventArgs e)
        {
            _expVisualQueue.Enqueue(new ExpVisualEvent(e.ExpData, true));
        }

        private void HandleTweensAnimations()
        {
            if (_expVisualQueue.Count > 0 && !IsPlayingLevelUpTween())
            {
                KillExpIncreaseTweenIfPlaying();
                PlayLevelUpTween();
            }
            else if (_lastQueuedExpInSameLevelIncreaseEvent.HasValue && CanPlayLastExpIncreaseTween())
            {
                KillExpIncreaseTweenIfPlaying();
                PlayLastExpIncreaseTween();
                _lastQueuedExpInSameLevelIncreaseEvent = null;
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
            ExpVisualEvent expEvent = _expVisualQueue.Dequeue();
            LevelData nextLevelData = expEvent.LevelData;

            float startExp = _expSlider.value;
            float endExp = _currentlyVisibleLevelData.MaxExp;

            if (Mathf.Approximately(startExp, endExp))
            {
                HandleLevelUpTransition(nextLevelData);
                return;
            }

            _levelUpTween = _expSlider.DOValue(endExp, CalculateSliderExpGainAnimSpeed(true))
                .SetEase(Ease.OutQuad)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    HandleLevelUpTransition(nextLevelData);
                });
        }

        private void HandleLevelUpTransition(LevelData nextLevelData)
        {
            _currentlyVisibleLevelData = nextLevelData;

            _expSlider.value = 0;
            _expSlider.maxValue = _currentlyVisibleLevelData.MaxExp;

            UpdateLevelText();

            OnExpSliderVisualEndValueReached?.Invoke(this, EventArgs.Empty);

            if (_expVisualQueue.Count > 0 && _expVisualQueue.Peek().IsLevelUp)
            {
                PlayLevelUpTween();
            }
            else
            {
                KillExpIncreaseTweenIfPlaying();
                if (_lastQueuedExpInSameLevelIncreaseEvent.HasValue && _lastQueuedExpInSameLevelIncreaseEvent.Value.LevelData.Lvl == _currentlyVisibleLevelData.Lvl)
                {
                    PlayLastExpIncreaseTween();
                    _lastQueuedExpInSameLevelIncreaseEvent = null;
                }
                else if (_currentlyVisibleLevelData.Exp > 0)
                {
                    _expIncreaseTween = AnimateSliderExpGain(_currentlyVisibleLevelData.Exp, false);
                }
            }
        }

        private void PlayLastExpIncreaseTween()
        {
            if (_lastQueuedExpInSameLevelIncreaseEvent.HasValue)
            {
                var expEvent = _lastQueuedExpInSameLevelIncreaseEvent.Value;
                _expIncreaseTween = AnimateSliderExpGain(expEvent.LevelData.Exp, false);
                _currentlyVisibleLevelData = expEvent.LevelData;
            }
        }

        private bool CanPlayLastExpIncreaseTween()
        {
            return !IsPlayingLevelUpTween()
                && _lastQueuedExpInSameLevelIncreaseEvent.HasValue
                && _lastQueuedExpInSameLevelIncreaseEvent.Value.LevelData.Lvl == _currentlyVisibleLevelData.Lvl;
        }

        private Tween AnimateSliderExpGain(float exp, bool isLevelUp)
        {
            float duration = CalculateSliderExpGainAnimSpeed(isLevelUp);
            return _expSlider
                .DOValue(exp, duration)
                .SetEase(Ease.OutQuad)
                .SetUpdate(true);
        }

        private float CalculateSliderExpGainAnimSpeed(bool isLevelUp)
        {
            if (isLevelUp)
            {
                int levelsToGo = _expVisualQueue.Count + 1;
                return Mathf.Min(FASTEST_EXP_INCREASE_ANIM_SPEED, BASE_EXP_INCREASE_ANIM_SPEED / Mathf.Max(1, levelsToGo));
            }
            else
            {
                return BASE_EXP_INCREASE_ANIM_SPEED;
            }
        }

        private void UpdateLevelText()
            => _levelText.text = $"{_currentlyVisibleLevelData.Lvl} Lvl";
    }
}

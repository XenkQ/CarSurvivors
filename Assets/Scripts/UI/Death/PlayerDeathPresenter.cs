using Assets.Scripts;
using Assets.Scripts.Audio;
using Assets.Scripts.Player;
using Assets.Scripts.UI;
using Assets.Scripts.Utils;
using Reflex.Attributes;
using TMPro;
using UnityEngine;

public sealed class PlayerDeathPresenter : MonoBehaviour
{
    [Inject] private readonly IBackgroundAudioManager _backgroundAudioManager;

    [SerializeField] private GameObject _visual;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _timeText;

    public static PlayerDeathPresenter Instace { get; private set; }

    private PlayerDeathPresenter()
    {
    }

    private void Awake()
    {
        if (Instace == null)
        {
            Instace = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _backgroundAudioManager.ChangeAudioToDefaultAudioMode();
    }

    public void EnableDeathScreen()
    {
        SetLevelText();

        SetTimeText();

        _visual.SetActive(true);

        _backgroundAudioManager.ChangeAudioToDeathAudioMode();

        GameTime.PauseTime();
    }

    private void SetLevelText()
    {
        _levelText.text = "Level: " + PlayerManager
            .Instance
            .LevelController
            .LevelData
            .Lvl
            .ToString();
    }

    private void SetTimeText()
    {
        _timeText.text = "Time Alive: " +
            TimeConversionUtility.FormatSecondsToTimeString(TimerPresenter.Instance.TimerValue);
    }
}
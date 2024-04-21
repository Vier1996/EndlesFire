using System.Collections.Generic;
using ACS.Core.ServicesContainer;
using ACS.SignalBus.SignalBus;
using Codebase.Library.Extension.Dotween;
using DG.Tweening;
using InternalAssets.Codebase.Gameplay.Configs;
using InternalAssets.Codebase.Gameplay.Enums;
using InternalAssets.Codebase.Signals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InternalAssets.Codebase.Gameplay.Entities.PlayerFolder.Progress
{
    public class PlayerProgressHandler : MonoBehaviour
    {
        [SerializeField] private Image _sliderProgress;
        [SerializeField] private TextMeshProUGUI _levelText;

        private Color _progressColor;
        
        private ISignalBusService _signalBusService;
        private PlayerProgressionSetup _currentSetup;
        
        private readonly List<ItemType> _availableItemTypes = new()
        {
            ItemType.game_experience_small,
            ItemType.game_experience_medium,
            ItemType.game_experience_big,
        };

        private int _level = 1;
        private long _currentExperience = 0;
        private bool _busyByIncreasing = false;
        
        private void Start()
        {
            ServiceContainer.Core.Get(out _signalBusService);
            
            Initialize();
        }
        
        private void Initialize()
        {
            _progressColor = _sliderProgress.color;
            
            UpdateProgressSetup();
            UpdateLevelText();
            
            _sliderProgress.fillAmount = 0;
            
            _signalBusService.Subscribe<InventoryItemRegistred>(OnPlayerRegisterNewItem);
        }

        private void OnDestroy()
        {
            _signalBusService.Unsubscribe<InventoryItemRegistred>(OnPlayerRegisterNewItem);
        }

        private void OnPlayerRegisterNewItem(InventoryItemRegistred signal)
        {
            if(_availableItemTypes.Contains(signal.Type) == false)
                return;

            _currentExperience += signal.Count;
            
            UpdateProgressView();
        }

        private void UpdateProgressSetup()
        {
            PlayerProgressionConfig config = PlayerProgressionConfig.GetInstance();

            _currentSetup = config.Get(_level);
            
            config.Release();
        }

        private void UpdateProgressView()
        {
            if (_currentExperience >= _currentSetup.NeededExperience)
            {
                IncreaseLevel();
                return;
            }

            float ratio = (float)_currentExperience / _currentSetup.NeededExperience;

            _sliderProgress.KillTween();
            _sliderProgress
                .DOFillAmount(ratio, 0.5f)
                .SetEase(Ease.InOutSine);
        }

        private void IncreaseLevel()
        {
            if(_busyByIncreasing) return;

            _busyByIncreasing = true;
            
            _sliderProgress.KillTween();
            _sliderProgress
                .DOFillAmount(1, 0.25f)
                .SetEase(Ease.InOutSine)
                .OnComplete(OnSliderFullFilled);
        }
        
        private void OnSliderFullFilled()
        {
            _level++;
            _currentExperience -= _currentSetup.NeededExperience;

            _sliderProgress
                .DOFade(0f, 0.2f)
                .OnComplete(() =>
                {
                    _sliderProgress.fillAmount = 0;
                    _sliderProgress.color = _progressColor;

                    CallUpgradeDialog();
                });
            
            UpdateProgressSetup();
            UpdateLevelText();
        }

        private void CallUpgradeDialog()
        {
            _busyByIncreasing = false;
            UpdateProgressView();
        }

        private void UpdateLevelText() => _levelText.text = $"Lvl. {_level.ToString()}";
    }
}
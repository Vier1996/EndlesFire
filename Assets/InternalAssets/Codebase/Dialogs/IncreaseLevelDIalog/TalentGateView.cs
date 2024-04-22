using System;
using Codebase.Library.Extension.MonoBehavior;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using InternalAssets.Codebase.Gameplay.Talents;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InternalAssets.Codebase.Dialogs.IncreaseLevelDialog
{
    public class TalentGateView : MonoBehaviour, IDisposable
    {
        private const float RemoveGatePositionY = -170f;
        
        [BoxGroup("General"), SerializeField] private Transform _gateImageTransform;
        [BoxGroup("General"), SerializeField] private TextMeshProUGUI _titleName;
        [BoxGroup("General"), SerializeField] private TextMeshProUGUI _description;
        [BoxGroup("General"), SerializeField] private Image _icon;
        [BoxGroup("General"), SerializeField] private Button _pickButton;
        
        [BoxGroup("Animation params"), SerializeField] private float _gateAnimationDuration = 0.5f;
        [BoxGroup("Animation params"), SerializeField] private float _displayButtonDuration = 0.5f;

        private TalentSetup _talentSetup;
        private void Awake() => _pickButton.transform.localScale = Vector3.zero;
    
        public void Dispose() => _pickButton.onClick.RemoveListener(PickTalent);

        public void SetupView(TalentSetup talentSetup)
        {
            _talentSetup = talentSetup;

            _titleName.text = _talentSetup.TalentNameKey;
            _description.text = _talentSetup.TalentDescriptionKey;
            _icon.sprite = _talentSetup.Icon;
        }
        
        public async UniTask RemoveGate() => 
            await _gateImageTransform.DOLocalMoveY(RemoveGatePositionY, _gateAnimationDuration)
                .SetUpdate(true)
                .AsyncWaitForCompletion();

        public async UniTask DisplayButton()
        {
            _pickButton.onClick.AddListener(PickTalent);
            
            await _pickButton.transform.DisplayBubbled(1.1f, _displayButtonDuration, defaultScale: 1f)
                .SetUpdate(true)
                .AsyncWaitForCompletion();
        }

        private void PickTalent()
        {
            
        }
    }
}

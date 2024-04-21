using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InternalAssets.Codebase.Dialogs.IncreaseLevelDialog
{
    public class TalentGateView : MonoBehaviour
    {
        [SerializeField] private Transform _gateImageTransform;
        
        [Button]
        private void RemoveGate()
        {
            _gateImageTransform.DOLocalMoveY(-170, 0.5f);
        }
    }
}

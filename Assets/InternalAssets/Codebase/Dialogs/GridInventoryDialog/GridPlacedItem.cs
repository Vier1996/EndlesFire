using System.Collections.Generic;
using Codebase.Library.Extension.Dotween;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace InternalAssets.Codebase.Dialogs.GridInventoryDialog
{
    public class GridPlacedItem : MonoBehaviour
    {
        [SerializeField] private Image _selfImage;
        
        public bool IsAvailable { get; private set; } = true;
        
        private List<GridInventorySlot> _busySlots = new();
        private RectTransform _selfRectTransform;
        private GridDraggableItem _item;
        
        private bool _isInitialized = false;
        private float _slotSizeX;
        private float _slotSizeY;
        private float _offset;

        public void Initialize(float slotSizeX, float slotSizeY, float offset)
        {
            if(_isInitialized) return;
            
            _isInitialized = true;
            
            _slotSizeX = slotSizeX;
            _slotSizeY = slotSizeY;
            _offset = offset;
            
            _selfRectTransform = transform as RectTransform;
        }

        [Button]
        public void BindItem(GridDraggableItem item, List<GridInventorySlot> busySlots)
        {
            _item = item;
            _busySlots = busySlots;
            
            _busySlots.ForEach(bs => bs.SetAsBusy());
            
            IsAvailable = false;
            
            gameObject.SetActive(!IsAvailable);

            NormalizePosition(item.Length, item.Width);
            
            _item.StartDrag += OnInteractionStarted;
            _item.CancelDrag += OnInteractionFinished;
        }

        [Button]
        public void UnbindItem()
        {
            _item.StartDrag -= OnInteractionStarted;
            _item.CancelDrag -= OnInteractionFinished;
            
            _busySlots.ForEach(bs => bs.SetAsAvailable());
            
            IsAvailable = true;
            
            gameObject.SetActive(!IsAvailable);
        }

        private void NormalizePosition(int length, int width)
        {
            Vector3 position = Vector3.zero;

            foreach (GridInventorySlot slot in _busySlots) 
                position += slot.transform.position;

            position /= _busySlots.Count;
            
            _selfRectTransform.position = position;
            _selfRectTransform.sizeDelta = new Vector2(
                x: _slotSizeX * width + (_offset * (width - 1)),
                y: _slotSizeY * length + (_offset * (length - 1))
            );
        }

        private void OnInteractionStarted()
        {
            SetLowColor();
        }

        private void OnInteractionFinished()
        {
            _item.Interrupt(_selfRectTransform);
            
            SetDefaultColor();
        }

        private void SetLowColor()
        {
            _selfImage.KillTween();
            _selfImage.DOFade(0.1f, 0.1f);
        }
        
        private void SetDefaultColor()
        {
            _selfImage.KillTween();
            _selfImage.DOFade(0.3f, 0.1f);
        }
    }
}

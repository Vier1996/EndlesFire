using System;
using Codebase.Library.Extension.Dotween;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace InternalAssets.Codebase.Dialogs.GridInventoryDialog
{
    public class GridInventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDisposable
    {
        public event Action<GridInventorySlot, GridDraggableItem> PointerEnter;
        public event Action<GridInventorySlot, GridDraggableItem> PointerExit;

        [SerializeField] private Image _selfImage;
        
        [BoxGroup, SerializeField] private Color _defaultColor;
        [BoxGroup, SerializeField] private Color _availablePlacementColor;
        [BoxGroup, SerializeField] private Color _notAvailablePlacementColor;

        public bool IsAvailable { get; private set; } = true;
        public int IndexI { get; private set; }
        public int IndexJ { get; private set; }

        public void Initialize(int i, int j)
        {
            IndexI = i;
            IndexJ = j;
        }
        
        public void Dispose()
        {
            
        }

        public void SetAsBusy() => gameObject.SetActive(IsAvailable = false);

        public void SetAsAvailable() => gameObject.SetActive(IsAvailable = true);

        public void EnableDefaultColor()
        {
            _selfImage.KillTween();
            _selfImage.DOColor(_defaultColor, 0.1f);
        }
        
        public void EnableAvailabilityColor(bool isAvailable)
        {
            _selfImage.KillTween();
            _selfImage.DOColor(isAvailable ? _availablePlacementColor : _notAvailablePlacementColor, 0.1f);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            GameObject eventObject = eventData.pointerDrag;
            GridDraggableItem draggableItem = null;
            
            if(eventObject != null)
                draggableItem = eventObject.GetComponent<GridDraggableItem>();
            
            PointerEnter?.Invoke(this, draggableItem);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            GameObject eventObject = eventData.pointerDrag;
            GridDraggableItem draggableItem = null;
            
            if(eventObject != null)
                draggableItem = eventObject.GetComponent<GridDraggableItem>();

            PointerExit?.Invoke(this, draggableItem);
        }
    }
}

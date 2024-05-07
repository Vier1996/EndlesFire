using System;
using Codebase.Library.Extension.Dotween;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Logger = InternalAssets.Codebase.Library.Logging.Logger;

namespace InternalAssets.Codebase.Dialogs.GridInventoryDialog
{
    public class GridDraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public event Action StartDrag; 
        public event Action CancelDrag; 
        
        public int Length = 1;
        public int Width = 1;
        
        [SerializeField] private Image _selfImage;
        
        private Transform _inDraggableStatusParent;
        private Transform _selfTransform;
        private Camera _camera;

        public void Initialize(Transform draggableParent)
        {
            _selfTransform = transform;
            _camera = Camera.main;
            _inDraggableStatusParent = draggableParent;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _selfTransform.KillTween();
            _selfTransform.SetParent(_inDraggableStatusParent);
            
            Logger.Log("Drag started");

            _selfImage.raycastTarget = false;
            
            StartDrag?.Invoke();
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector3 nextPosition = _camera.ScreenToWorldPoint(Input.mousePosition);

            nextPosition.z = 0;
            
            _selfTransform.position = nextPosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Logger.Log("Drag finished");
            
            _selfImage.raycastTarget = true;
            
            CancelDrag?.Invoke();
        }

        public void Interrupt(Transform slotTransform)
        {
            _selfTransform.KillTween();
            _selfTransform.SetParent(slotTransform);
            _selfTransform.DOLocalMove(Vector3.zero, 0.15f);
        }
    }
}

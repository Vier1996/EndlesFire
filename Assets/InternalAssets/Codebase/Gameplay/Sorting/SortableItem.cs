using System.Collections.Generic;
using ACS.Core.ServicesContainer;
using UnityEngine;

namespace Codebase.Gameplay.Sorting
{
    public class SortableItem : MonoBehaviour, ISortableItem
    {
        [SerializeField] private Transform _anchor;
        [SerializeField] private List<Renderer> _customRenderers;
        
        public int LastOrder => _order;
        
        private float _globalY = 0f;
        private float _ratio = 0f;
        private int _order = 0;
        
        private SortingService _sortingService;
        private SortingService.SortingParameters _sortingParameters;
        
        private void Start()
        {
            if (_sortingService == null)
            {
                ServiceContainer.Global.Get(out _sortingService);
                
                _sortingParameters = _sortingService.Subscribe(this);
                
                NormalizeSortingLayer();
            }
        }
        
        public void AddRenderers(List<Renderer> renderers)
        {
            for (int i = 0; i < renderers.Count; i++) 
                _customRenderers.Add(renderers[i]);

            NormalizeSortingLayer();
        }
        
        public void RemoveRenderers(List<Renderer> renderers)
        {
            for (int i = 0; i < renderers.Count; i++)
            {
                if (_customRenderers.Contains(renderers[i]))
                    _customRenderers.Remove(renderers[i]);
            }

            NormalizeSortingLayer();
        }
        
        public bool IsAnchorValid() => _anchor != null;

        public void UpdateOrder()
        {
            if(gameObject != null && !gameObject.activeInHierarchy)
                return;

            _globalY = _anchor.position.y;

            if (!IsAnchorValid()) 
                _sortingService.Unsubscribe(this);
                
            if (_globalY > _sortingParameters.UpperLimit) _globalY = _sortingParameters.UpperLimit;
            if (_globalY < _sortingParameters.LowerLimit) _globalY = _sortingParameters.LowerLimit;

            _globalY += _sortingParameters.Offset;
            _ratio = 1f - (_globalY / _sortingParameters.LimitsSum);
            _order = (int) (_sortingParameters.SortingSensitivityOrderValue * _ratio);
            
            SetupOrder(_order);
        }
        
        private void SetupOrder(int order)
        {
            for (int i = 0; i < _customRenderers.Count; i++)
            {
                if (_customRenderers[i] != null) 
                    _customRenderers[i].sortingOrder = order;
            }
        }

        private void NormalizeSortingLayer()
        {
            for (int i = 0; i < _customRenderers.Count; i++)
            {
                if (_customRenderers[i] != null) 
                    _customRenderers[i].sortingLayerID = 0;
            }
        }

        private void OnDestroy() => 
            _sortingService.Unsubscribe(this);
    }

    public interface ISortableItem
    {
        void UpdateOrder();
        bool IsAnchorValid();
    }
}

using System;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.HealthLogic
{
    public class HealthHitComponent : MonoBehaviour
    {
        [BoxGroup("Renderer")] [SerializeField] private Material _hitMaterial;
        [BoxGroup("Renderer")] [SerializeField] private Renderer[] _hitableRenderers;
        [BoxGroup("TextView")] [SerializeField] private Transform _damageTextParent;
        [BoxGroup("SpeedParameter")] [SerializeField] private float _changeSpeed = 0.05f;

        private Material[] _defaultMaterials;
        private int _damageValue = 0;
        private HealthColorVariation _healthColorVariation;
        
        private void Start() => BindMaterials();

        public void AppendRenderer(Renderer newRenderer)
        {
            Renderer[] cachedRenderers = _hitableRenderers;
            _hitableRenderers = new Renderer[cachedRenderers.Length + 1];

            for (int i = 0; i < cachedRenderers.Length; i++) 
                _hitableRenderers[i] = cachedRenderers[i];

            _hitableRenderers[cachedRenderers.Length] = newRenderer;
        }
        
        public HealthHitComponent SetValue(int value, HealthColorVariation healthColorVariation)
        {
            _damageValue = value;
            _healthColorVariation = healthColorVariation;
            return this;
        }
        
        public async void PlayHitAnimation()
        {
            for (int i = 0; i < _hitableRenderers.Length; i++) 
                SetBlinkMaterial(_hitableRenderers[i]);
            
            await UniTask.Delay(TimeSpan.FromSeconds(_changeSpeed));
            
            for (int i = 0; i < _hitableRenderers.Length; i++) 
                ReturnMaterials();
        }

        private void SetBlinkMaterial(Renderer targetRenderer) => 
            targetRenderer.material = _hitMaterial;
        
        private void ReturnMaterials()
        {
            for (int i = 0; i < _hitableRenderers.Length; i++)
            {
                if(_hitableRenderers[i] != null)   
                    _hitableRenderers[i].material = _defaultMaterials[i];
            }
        }

        private void BindMaterials()
        {
            if (_hitableRenderers == null) return;

            _defaultMaterials = new Material[_hitableRenderers.Length];
            
            for (int i = 0; i < _hitableRenderers.Length; i++) 
                _defaultMaterials[i] = _hitableRenderers[i].material;
        }
    }
}
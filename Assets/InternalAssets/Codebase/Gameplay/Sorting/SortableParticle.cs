using System;
using System.Collections.Generic;
using System.Linq;
using Lean.Pool;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Sorting
{
    public class SortableParticle : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;
        //[SerializeField, ReadOnly] private List<Renderer> _renderers;

        public virtual void Play() => _particleSystem.Play();

        /*public void ChangeSortingOrder(int order)
        {
            foreach (Renderer render in _renderers)
                render.sortingOrder = order;
        }*/

        private void OnParticleSystemStopped() => LeanPool.Despawn(gameObject);

#if UNITY_EDITOR
        //[Button]
        //private void GetAllRenderers() => 
        //    _renderers = _particleSystem.GetComponentsInChildren<Renderer>().ToList();
#endif
    }
}

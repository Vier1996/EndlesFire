using InternalAssets.Codebase.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.ProgressTimers
{
    public abstract class ProgressTimerView : MonoBehaviour, IBootstrappable<ProgressTimerView>, IRecycledClass<ProgressTimerView>
    {
        private const string ProgressShaderProperty = "_Arc2";

        [BoxGroup("Renderer"), SerializeField] private Renderer _selfRenderer;

        private Material _progressMaterial;
        
        public ProgressTimerView Bootstrapp()
        {
            _progressMaterial = _selfRenderer.material;
            
            return this;
        }

        public ProgressTimerView Enable()
        {
            return this;
        }

        public ProgressTimerView Disable()
        {
            return this;
        }
    }
}

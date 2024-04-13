using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.ModelsView
{
    public class ModelViewProvider : MonoBehaviour
    {
        [field: SerializeField] public ModelView ModelView { get; private set; }

        private void Start()
        {
            if(ModelView != null)
                ModelView.SpriteSheetAnimator.Bootstrapp();
        }

        public void ReplaceView(ModelView modelView)
        {
            if(ModelView != null)
                Destroy(ModelView.gameObject);

            ModelView = Instantiate(modelView, transform);
            ModelView.SpriteSheetAnimator.Bootstrapp();
        }

        private void OnDestroy()
        {
            if(ModelView != null)
                ModelView.SpriteSheetAnimator.Dispose();
        }
    }
}

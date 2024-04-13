using InternalAssets.Codebase.Services._2dModels;
using InternalAssets.Codebase.Services.Animation;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.ModelsView
{
    public class ModelView : MonoBehaviour
    {
        [field: SerializeField] public SpriteSheetAnimator SpriteSheetAnimator { get; private set; }
        [field: SerializeField] public SpriteModelPresenter SpriteModelPresenter { get; private set; }
    }
}
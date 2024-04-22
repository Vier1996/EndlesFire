using Codebase.Library.Extension.MonoBehavior;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace InternalAssets.Codebase.Dialogs.Extras
{
    public class DisplayableParent : MonoBehaviour
    {
        public async UniTask Display(float expandScale, float defaultScale, float duration)
        {
            transform.localScale = Vector3.zero;
            
            await transform.DisplayBubbled(expandScale, duration, defaultScale: defaultScale)
                .SetUpdate(true)
                .AsyncWaitForCompletion();
        }
    }
}

using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.HUDs
{
    public abstract class HudPresenter : MonoBehaviour
    {
        [BoxGroup("Elements"), SerializeField] private List<HudElement> _hudElements = new();

        public HudPresenter Initialize()
        {
            _hudElements.ForEach(el => el.Initialize());
            return this;
        }
        
        public HudPresenter Enable()
        {
            gameObject.SetActive(true);
            return this;
        }
        
        public HudPresenter Disable() 
        {
            gameObject.SetActive(false);
            return this;
        }
        
        public async UniTask Display(bool instant = false)
        {
            float duration = 0;
            
            foreach (HudElement element in _hudElements)
            {
                float currentDuration = element.Enable(instant);

                if (currentDuration > duration)
                    duration = currentDuration;
            }
            
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
        }

        public async UniTask Hide(bool instant = false)
        {
            float duration = 0;
            
            foreach (HudElement element in _hudElements)
            {
                float currentDuration = element.Disable(instant);

                if (currentDuration > duration)
                    duration = currentDuration;
            }
            
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
        }
    }
}
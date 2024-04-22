using System.Collections.Generic;
using ACS.Dialog.Dialogs.View;
using InternalAssets.Codebase.Dialogs.Extras;
using InternalAssets.Codebase.Gameplay.Enums;
using InternalAssets.Codebase.Gameplay.Talents;
using UnityEngine;

namespace InternalAssets.Codebase.Dialogs.IncreaseLevelDialog
{
    public class IncreaseLevelDialog : DialogView
    {
        [SerializeField] private DisplayableParent _displayableParent;
        [SerializeField] private List<TalentGateView> _talentGateViews = new();
        
        public override async void Show()
        {
            base.Show();
            
            await _displayableParent.Display(1f, 0.9f, 0.5f);

            Initialize();
        }
        
        private async void Initialize()
        {
            TalentsConfig config = await TalentsConfig.GetInstanceAsync();
            
            int index = 1;
            
            foreach (TalentGateView view in _talentGateViews)
            {
                view.SetupView(config.Get((TalentType)index));
                
                await view.RemoveGate();

                index++;
            }
            
            foreach (TalentGateView view in _talentGateViews) 
                await view.DisplayButton();
            
            config.Release();
        }
    }
}
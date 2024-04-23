using System.Collections.Generic;
using ACS.Core.ServicesContainer;
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

        private TalentsService _talentsService;
        
        public override async void Show()
        {
            base.Show();

            ServiceContainer.ForCurrentScene().Get(out _talentsService);
            
            await _displayableParent.Display(1f, 0.9f, 0.5f);

            Initialize();
        }
        
        private async void Initialize()
        {
            int index = 0;
            List<TalentType> chosenTalent = await _talentsService.Generator.Generate();
            
            foreach (TalentGateView view in _talentGateViews)
            {
                view.SetupView(_talentsService.GetTalentSetup(chosenTalent[index]));
                
                await view.RemoveGate();

                index++;
            }
            
            foreach (TalentGateView view in _talentGateViews) 
                await view.DisplayButton();
        }
    }
}
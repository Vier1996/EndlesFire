using System;
using ACS.Core.ServicesContainer;
using ACS.Dialog.Dialogs;
using ACS.Dialog.Dialogs.Arguments;
using InternalAssets.Codebase.Dialogs.IncreaseLevelDialog;
using InternalAssets.Codebase.Gameplay.Enums;
using InternalAssets.Codebase.Gameplay.Talents;
using Sirenix.OdinInspector;

namespace InternalAssets.Codebase.DebugFolder
{
    public class DebugEditor : SerializedMonoBehaviour
    {
        private IDialogService _dialogService;
        private TalentsService _talentsService;
        
        private void Start()
        {
            ServiceContainer.Core.Get(out _dialogService);
            ServiceContainer.ForCurrentScene().Get(out _talentsService);
        }

        [Button]
        private void CallDialog(Type dialogType, DialogArgs dialogArgs = null) => _dialogService.CallDialog(dialogType);

        [Button]
        private void CallIncreaseLevelDialog() => _dialogService.CallDialog(typeof(IncreaseLevelDialog));

        [Button]
        private void ApplyTalent(TalentType talentType) => _talentsService.ApplyTalent(talentType);
    }
}

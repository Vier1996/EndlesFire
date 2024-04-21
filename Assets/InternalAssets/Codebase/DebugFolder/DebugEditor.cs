using System;
using ACS.Core.ServicesContainer;
using ACS.Dialog.Dialogs;
using ACS.Dialog.Dialogs.Arguments;
using ACS.Dialog.Dialogs.View;
using Sirenix.OdinInspector;

namespace InternalAssets.Codebase.DebugFolder
{
    public class DebugEditor : SerializedMonoBehaviour
    {
        private IDialogService _dialogService;
        
        private void Start()
        {
            ServiceContainer.Core.Get(out _dialogService);
        }

        [Button]
        private void CallDialog(Type dialogType, DialogArgs dialogArgs = null)
        {
            _dialogService.CallDialog(dialogType);
        }
    }
}

using System;
using ACS.Dialog.Dialogs.Arguments;
using ACS.Dialog.Dialogs.View;
using InternalAssets.Codebase.Dialogs.Extras;
using UnityEngine;
using UnityEngine.UI;

namespace InternalAssets.Codebase.Dialogs.ChangeWeaponDialog
{
    public class ChangeWeaponDialog : DialogView
    {
        [SerializeField] private DisplayableParent _displayableParent;
        [SerializeField] private Button _changeButton;
        [SerializeField] private Button _ignoreButton;

        private ChangeWeaponDialogArgs _changeWeaponDialogArgs;
        
        private bool _status = false;
        
        public override async void Show()
        {
            base.Show();

            _changeWeaponDialogArgs = GetArgs<ChangeWeaponDialogArgs>();
            
            await _displayableParent.Display(1f, 0.9f, 0.5f);
            
            Initialize();
        }

        private void Initialize()
        {
            _changeButton.onClick.AddListener(() => OnPicked(true));
            _ignoreButton.onClick.AddListener(() => OnPicked(false));
        }

        private void OnPicked(bool status)
        {
            _changeButton.onClick.RemoveListener(() => OnPicked(true));
            _ignoreButton.onClick.RemoveListener(() => OnPicked(false));

            _status = status;
            
            Hide();
        }

        public override void Hide()
        {
            _changeWeaponDialogArgs.ChooseCallback?.Invoke(_status);

            base.Hide();
        }
    }
    
    public class ChangeWeaponDialogArgs : DialogArgs
    {
        public Action<bool> ChooseCallback;
    }
}

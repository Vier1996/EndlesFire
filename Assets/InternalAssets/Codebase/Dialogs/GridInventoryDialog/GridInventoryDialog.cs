using ACS.Dialog.Dialogs.View;
using UnityEngine;

namespace InternalAssets.Codebase.Dialogs.GridInventoryDialog
{
    public class GridInventoryDialog : DialogView
    {
        [SerializeField] private Grid _inventoryGrid;
        [SerializeField] private Grid _lootGrid;
        
        public override void Show()
        {
            base.Show();
            
            Initialize();
        }

        private void OnDestroy()
        {
            _inventoryGrid.Dispose();
            _lootGrid.Dispose();
        }

        private void Initialize()
        {
            _inventoryGrid.Initialize(2, 4);
            _lootGrid.Initialize(2, 4);
        }
    }
}

using ACS.SignalBus.SignalBus.Parent;
using InternalAssets.Codebase.Gameplay.Enums;

namespace InternalAssets.Codebase.Signals
{
    [Signal]
    public struct InventoryItemRegistred
    {
        public ItemType Type { get; private set; }
        public long Count { get; private set; }

        public InventoryItemRegistred(ItemType type, long count)
        {
            Type = type;
            Count = count;
        }
    }
}
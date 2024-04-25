
namespace InternalAssets.Codebase.Interfaces
{
    public interface IItemData
    {
        public bool CanBePlacedToInventory { get; }
        public bool CanBeSignaled { get; }
    }
}
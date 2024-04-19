using Cysharp.Threading.Tasks;
using InternalAssets.Codebase.Gameplay.Enums;
using InternalAssets.Codebase.Gameplay.Items;
using InternalAssets.Codebase.Library.Design.Factory;

namespace InternalAssets.Codebase.Gameplay.Factory.itemViews
{
    public class ItemViewsFactoryProvider : AbstractFactory<ItemType, ItemView>
    {
        protected override IFactory<ItemView, ItemType> GetSetup() => ItemViewsFactory.GetInstance();
        protected override async UniTask<IFactory<ItemView, ItemType>> GetSetupAsync() => await ItemViewsFactory.GetInstanceAsync();
        protected override void ReleaseSetup() => ItemViewsFactory.ReleaseInstance();
    }
}
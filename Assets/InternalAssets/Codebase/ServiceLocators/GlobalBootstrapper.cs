using ACS.Core.ServicesContainer;
using Codebase.Gameplay.Sorting;
using InternalAssets.Codebase.Gameplay.Factory.itemViews;
using InternalAssets.Codebase.Gameplay.Factory.Vfx;

namespace InternalAssets.Codebase.ServiceLocators
{
    public class GlobalBootstrapper : ServiceContainerGlobal
    {
        protected override void Bootstrap()
        {
            Container.Register(new SortingService());

            RegisterFactories();
            
            Container.AsGlobal(true);
        }

        private void RegisterFactories() =>
            Container
                .Register(new ItemViewsFactoryProvider())
                .Register(new VfxFactoryProvider());
    }
}

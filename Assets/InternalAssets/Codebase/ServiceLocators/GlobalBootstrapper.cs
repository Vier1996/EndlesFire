using ACS.Core.ServicesContainer;
using Codebase.Gameplay.Sorting;
using Codebase.Library.SAD;

namespace InternalAssets.Codebase.ServiceLocators
{
    public class GlobalBootstrapper : ServiceContainerGlobal
    {
        protected override void Bootstrap()
        {
            Container
                .Register(new SortingService())
                ;
            
            Container.AsGlobal(true);
        }
    }
}

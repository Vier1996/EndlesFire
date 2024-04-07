using ACS.Core.ServicesContainer;
using Codebase.Gameplay.Sorting;
using Codebase.Library.SAD;

namespace InternalAssets.Codebase.ServiceLocators
{
    public class GlobalLocator : Bootstrapper
    {
        protected override void Bootstrap()
        {
            Container
                .Register(new EntityWorld())
                .Register(new SortingService())
                ;
            
            Container.AsGlobal(true);
        }
    }
}

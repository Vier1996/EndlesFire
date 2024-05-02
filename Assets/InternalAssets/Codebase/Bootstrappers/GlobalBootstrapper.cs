using ACS.Core.ServicesContainer;
using Codebase.Gameplay.Sorting;
using InternalAssets.Codebase.Gameplay.Factory.itemViews;
using InternalAssets.Codebase.Gameplay.Factory.Vfx;
using InternalAssets.Codebase.Gameplay.SceneLoader;
using InternalAssets.Codebase.Gameplay.Shutters;
using InternalAssets.Codebase.Gameplay.SkillsTree;
using InternalAssets.Codebase.Interfaces;
using InternalAssets.Codebase.Services.Scenes;
using UnityEngine;

namespace InternalAssets.Codebase.Bootstrappers
{
    public class GlobalBootstrapper : ServiceContainerGlobal
    {
        [SerializeField] private Shutter _shutter;
        
        protected override void Bootstrap()
        {
            Container.AsGlobal(true);

            Container
                .Register(typeof(IShutter), _shutter)
                .Register(new SortingService())
                .Register(new SkillsService())
                .Register(typeof(ISceneLoader), new EndlessFireSceneLoader())
                ;

            RegisterFactories();
        }

        private void RegisterFactories() =>
            Container
                .Register(new ItemViewsFactoryProvider())
                .Register(new VfxFactoryProvider())
            ;
    }
}

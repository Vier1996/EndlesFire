using ACS.Core.ServicesContainer;
using Codebase.Library.SAD;
using InternalAssets.Codebase.Gameplay.Entities.PlayerFolder;
using InternalAssets.Codebase.Services.Camera;
using UnityEngine;

namespace InternalAssets.Codebase.ServiceLocators
{
    public class PolygonSceneBootstrapper : ServiceContainerLocal
    {
        [SerializeField] private Player _player;
        [SerializeField] private CameraCenterer _cameraCenterer;
        
        protected override void Bootstrap()
        {
            Container
                .Register(_player)
                .Register(new EntityWorld())
                .AsScene();

            BootstrappScene();
        }

        private void BootstrappScene()
        {
            _player.Bootstrapp();
            _cameraCenterer.Initialize(_player);
        }
    }
}

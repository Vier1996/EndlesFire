using ACS.Core.ServicesContainer;
using InternalAssets.Codebase.Gameplay.Entities.PlayerFolder;
using InternalAssets.Codebase.Library.MonoEntity;
using InternalAssets.Codebase.Library.MonoEntity.Tools.World;
using InternalAssets.Codebase.Services.Camera;
using Lean.Pool;
using UnityEngine;

namespace InternalAssets.Codebase.Bootstrappers
{
    public class PolygonSceneBootstrapper : ServiceContainerLocal
    {
        [SerializeField] private Player _player;
        [SerializeField] private Transform _playerParent;
        
        [SerializeField] private CameraCenterer _cameraCenterer;
        
        protected override void Bootstrap()
        {
            Container
                .Register(new EntityWorld())
                .AsScene();

            BootstrappScene();
        }

        private void BootstrappScene()
        {
            Player player = BootstrapPlayer();

            Container.Register(player);
            
            _cameraCenterer.Initialize(player);
        }

        private Player BootstrapPlayer()
        {
            Player player = LeanPool.Spawn(_player, _playerParent);

            player.Bootstrapp();
            player.Initialize();

            return player;
        }
    }
}

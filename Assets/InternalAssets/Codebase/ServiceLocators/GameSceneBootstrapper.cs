using ACS.Core.ServicesContainer;
using Codebase.Library.SAD;
using InternalAssets.Codebase.Gameplay.Directors;
using InternalAssets.Codebase.Gameplay.Entities.PlayerFolder;
using InternalAssets.Codebase.Gameplay.Map.Floor;
using InternalAssets.Codebase.Gameplay.Parents;
using InternalAssets.Codebase.Gameplay.Spawners;
using InternalAssets.Codebase.Services.Camera;
using Lean.Pool;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InternalAssets.Codebase.ServiceLocators
{
    public class GameSceneBootstrapper : ServiceContainerLocal
    {
        [BoxGroup("Components"), SerializeField] private SceneAssetParentsContainer _sceneAssetParentsContainer;
        [BoxGroup("Components"), SerializeField] private RecycledFloor _recycledFloor;
        [BoxGroup("Components"), SerializeField] private AbstractSpawner _enemiesSpawner;
        [BoxGroup("Components"), SerializeField] private GameplayDirectorsBootstrapper _gameplayDirectorsBootstrapper;
        
        [BoxGroup("Player"), SerializeField] private Player _playerPrefab;
        
        [SerializeField] private CameraCenterer _cameraCenterer;
        
        protected override void Bootstrap()
        {
            Container
                .Register(new EntityWorld())
                .Register(_sceneAssetParentsContainer)
                .AsScene();

            Player player = BootstrapPlayer();

            Container.Register(player);
            
            SetupCamera(player);
            SetupRecycledFloor(player);
            SetupEnemiesSpawner(player);
            SetupGameplayDirector(player);
        }
        
        private Player BootstrapPlayer()
        {
            Player player = LeanPool.Spawn(_playerPrefab, _sceneAssetParentsContainer.PlayerParent);

            player.Bootstrapp();
            player.Initialize();

            return player;
        }

        private void SetupCamera(Entity listeningEntity) => _cameraCenterer.Initialize(listeningEntity);
        
        private void SetupRecycledFloor(Entity listeningEntity) => _recycledFloor.Initialize(listeningEntity);
        
        private void SetupEnemiesSpawner(Entity listeningEntity) => _enemiesSpawner.Initialize(listeningEntity);
        private void SetupGameplayDirector(Entity listeningEntity) => _gameplayDirectorsBootstrapper.Initialize(listeningEntity);
    }
}
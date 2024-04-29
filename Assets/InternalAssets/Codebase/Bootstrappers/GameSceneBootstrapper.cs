using ACS.Core.ServicesContainer;
using InternalAssets.Codebase.Gameplay.Directors;
using InternalAssets.Codebase.Gameplay.Entities.PlayerFolder;
using InternalAssets.Codebase.Gameplay.HUDs;
using InternalAssets.Codebase.Gameplay.Map.Floor;
using InternalAssets.Codebase.Gameplay.Parents;
using InternalAssets.Codebase.Gameplay.Spawners;
using InternalAssets.Codebase.Gameplay.Talents;
using InternalAssets.Codebase.Library.MonoEntity;
using InternalAssets.Codebase.Library.MonoEntity.Entities;
using InternalAssets.Codebase.Library.MonoEntity.Tools.World;
using InternalAssets.Codebase.Services.Camera;
using Lean.Pool;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InternalAssets.Codebase.Bootstrappers
{
    public class GameSceneBootstrapper : ServiceContainerLocal
    {
        [BoxGroup("Components"), SerializeField] private SceneAssetParentsContainer _sceneAssetParentsContainer;
        [BoxGroup("Components"), SerializeField] private RecycledFloor _recycledFloor;
        [BoxGroup("Components"), SerializeField] private AbstractSpawner _enemiesSpawner;
        [BoxGroup("Components"), SerializeField] private GameplayDirectorsBootstrapper _gameplayDirectorsBootstrapper;
        [BoxGroup("Components"), SerializeField] private CameraCenterer _cameraCenterer;
        [BoxGroup("Player"), SerializeField] private Player _playerPrefab;
        [BoxGroup("HUD"), SerializeField] private HUD _hud;
        
        protected override void Bootstrap()
        {
            Container.AsScene();
            
            Container
                .Register(new EntityWorld())
                .Register(new TalentsService())
                .Register(_sceneAssetParentsContainer)
                .AsScene();

            Player player = BootstrapPlayer();

            Container.Register(player);
            
            SetupCamera(player);
            SetupRecycledFloor(player);
            SetupEnemiesSpawner(player);
            SetupGameplayDirector(player);
        }

        private void Start()
        {
            SetupHud();
        }

        private void OnDisable()
        {
            Container.Dispose();
            _hud.Dispose();
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

        private void SetupHud() => _hud.Bootstrapp();
    }
}
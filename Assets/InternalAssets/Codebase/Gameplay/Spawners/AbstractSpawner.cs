﻿using InternalAssets.Codebase.Library.MonoEntity;
using InternalAssets.Codebase.Library.MonoEntity.Entities;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Spawners
{
    public abstract class AbstractSpawner : MonoBehaviour, IEnemiesSpawner
    { 
        public abstract void Initialize(Entity listeningEntity);
        public abstract void Spawn();
    }
}
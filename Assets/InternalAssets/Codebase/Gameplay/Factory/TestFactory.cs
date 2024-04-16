using InternalAssets.Codebase.Gameplay.Items;
using Lean.Pool;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Factory
{
    public class TestFactory : SerializedMonoBehaviour
    {
        [OdinSerialize] private ItemData _data;

        [SerializeField] private ItemView _targetItem;
        [SerializeField] private Transform _spawnPoint;

        [Button]
        private void Spawn()
        {
            ItemView item = LeanPool.Spawn(_targetItem);

            item
                .Bootstrapp()
                .Setup(_data, _spawnPoint.position);
        }
    }
}

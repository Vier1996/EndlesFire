using Codebase.Library.SAD;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Entities.PlayerFolder
{
    public class Player : Entity
    {
        [SerializeField] private PlayerComponents _playerComponents;

        [Button]
        public override Entity Bootstrapp() => base.Bootstrapp().BindComponents(_playerComponents);
    }
}

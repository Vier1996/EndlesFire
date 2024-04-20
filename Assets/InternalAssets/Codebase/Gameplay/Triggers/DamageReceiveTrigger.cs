using InternalAssets.Codebase.Interfaces;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace InternalAssets.Codebase.Gameplay.Triggers
{
    public class DamageReceiveTrigger : SerializedMonoBehaviour, ITrigger
    {
        [field: OdinSerialize] public IDamageReceiver DamageReceiver { get; private set; }
    }
}

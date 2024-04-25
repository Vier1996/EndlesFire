using ACS.SignalBus.SignalBus.Parent;
using InternalAssets.Codebase.Gameplay.Enums;

namespace InternalAssets.Codebase.Signals
{
    [Signal]
    public struct WeaponItemRegistred
    {
        public WeaponType Type { get; private set; }

        public WeaponItemRegistred(WeaponType type) => Type = type;
    }
}
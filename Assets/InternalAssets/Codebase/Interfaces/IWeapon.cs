namespace InternalAssets.Codebase.Interfaces
{
    public interface IWeapon
    {
        public void StartFire(ITargetable target);
        public void StopFire();
    }
}
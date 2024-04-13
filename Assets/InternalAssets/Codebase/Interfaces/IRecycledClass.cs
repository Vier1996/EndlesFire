namespace InternalAssets.Codebase.Interfaces
{
    public interface IRecycledClass<out T>
    {
        T Enable();
        T Disable();
    }
}
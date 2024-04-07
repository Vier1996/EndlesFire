namespace InternalAssets.Codebase.Interfaces
{
    public interface IBootstrappable<out T>
    {
        public T Bootstrapp();
    }
}
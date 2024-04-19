namespace InternalAssets.Codebase.Library.Design.Factory
{
    public interface IFactory<out TFactoryValue, in TFactoryType>
    {
        public TFactoryValue GetFactoryValue(TFactoryType factoryType);
    }
}
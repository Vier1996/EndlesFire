namespace InternalAssets.Codebase.Library.GameConditions
{
    public interface IGameCondition
    {
        public GameConditionStatus IsValid();
    }
}
using System.Collections.Generic;
using System.Linq;

namespace InternalAssets.Codebase.Library.GameConditions
{
    public static class GameConditionExtension
    {
        public static bool Resolve<T>(this IList<T> list) where T : IGameCondition => 
            list.All(t => t.IsValid() != GameConditionStatus.Failure);
    }
}
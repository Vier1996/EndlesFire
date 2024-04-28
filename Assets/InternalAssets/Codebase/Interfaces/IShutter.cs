using System;
using Cysharp.Threading.Tasks;

namespace InternalAssets.Codebase.Interfaces
{
    public interface IShutter
    {
        public event Action ShutterOpen;
        public event Action ShutterClosed;
        public UniTask DisplayShutter();
        public void HideShutter();
    }
}
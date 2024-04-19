using Cysharp.Threading.Tasks;
using InternalAssets.Codebase.Gameplay.Enums;
using InternalAssets.Codebase.Gameplay.Sorting;
using InternalAssets.Codebase.Library.Design.Factory;

namespace InternalAssets.Codebase.Gameplay.Factory.Vfx
{
    public class VfxFactoryProvider : AbstractFactory<VfxType, SortableParticle>
    {
        protected override IFactory<SortableParticle, VfxType> GetSetup() => VfxFactory.GetInstance();
        protected override async UniTask<IFactory<SortableParticle, VfxType>> GetSetupAsync() => await VfxFactory.GetInstanceAsync();
        protected override void ReleaseSetup() => VfxFactory.ReleaseInstance();
    }
}
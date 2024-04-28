using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace InternalAssets.Codebase.Gameplay.HUDs
{
    public class HUD : MonoBehaviour
    {
        [BoxGroup("Presenters"), SerializeField] private HudPresenter _menuHud;
        [BoxGroup("Presenters"), SerializeField] private HudPresenter _gameHud;
        
        [BoxGroup("Buttons"), SerializeField] private Button _playButton;

        public async void Bootstrapp()
        {
            _menuHud.Initialize();
            _gameHud.Initialize();
            
            DisableGameHud();
            
            await HideMenuHud(instant: true);
            await HideGameHud(instant: true);

            await DisplayMenuHud();
            
            _playButton.onClick.AddListener(PlayCallback);
        }

        public void Dispose()
        {
            _playButton.onClick.RemoveListener(PlayCallback);
        }
        
        private HudPresenter EnableMenuHud() => _menuHud.Enable();
        private HudPresenter DisableMenuHud() => _menuHud.Disable();
        private async UniTask DisplayMenuHud(bool instant = false) => await _menuHud.Display(instant);
        private async UniTask HideMenuHud(bool instant = false) => await _menuHud.Hide(instant);
        
        private HudPresenter EnableGameHud() => _gameHud.Enable();
        private HudPresenter DisableGameHud() => _gameHud.Disable();
        private async UniTask DisplayGameHud(bool instant = false) => await _gameHud.Display(instant);
        private async UniTask HideGameHud(bool instant = false) => await _gameHud.Hide(instant);

        private async void PlayCallback()
        {
            await HideMenuHud();

            EnableGameHud();

            await DisplayGameHud();
        }
    }
}

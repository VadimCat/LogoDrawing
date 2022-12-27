using Client.UI.Screens;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Utils;

namespace Client.Screens
{
    public class AppLoadingScreen : BaseScreen
    {
        [SerializeField] private Transform logo0;
        [SerializeField] private TextLoadingBar loadingBar;

        private void Awake()
        {
            AnimateLogo();
        }

        private void AnimateLogo()
        {
            logo0.DoPulseScale(1.04f, 1, gameObject);
        }

        public void SetProgress(float progress)
        {
            loadingBar.SetLoadingProgress(progress);
        }

        public async UniTask AnimateLoadingBar(float duration)
        {
            await loadingBar.AnimateProgress(duration);
        }
    }
}
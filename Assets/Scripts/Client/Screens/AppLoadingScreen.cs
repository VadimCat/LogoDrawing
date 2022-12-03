using UnityEngine;
using DG.Tweening;

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
            logo0.DOScale(1.04f, 1)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.Linear)
                .SetLink(gameObject);
        }

        public void SetProgress(float progress)
        {
            loadingBar.SetLoadingProgress(progress);
        }
    }
}
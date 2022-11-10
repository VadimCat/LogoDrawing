using UnityEngine;

namespace Client.Screens
{
    public class AppLoadingScreen : BaseScreen
    {
        [SerializeField] private TextLoadingBar loadingBar;

        public void SetProgress(float progress)
        {
            loadingBar.SetLoadingProgress(progress);
        }
    }
}
using Client.Screens;
using Cysharp.Threading.Tasks;

namespace Client
{
    public class LoadingPresenter
    {
        private readonly ScreenNavigator screenNavigator;
        private readonly BackGroundService backGroundService;
        private readonly LevelService levelService;

        private AppLoadingScreen loadingScreen;

        public LoadingPresenter(ScreenNavigator screenNavigator, BackGroundService backGroundService,
            LevelService levelService)
        {
            this.screenNavigator = screenNavigator;
            this.backGroundService = backGroundService;
            this.levelService = levelService;
        }

        public async UniTask Load()
        {
            backGroundService.SwitchBackground(Background.Loading);
            loadingScreen = await screenNavigator.PushScreen<AppLoadingScreen>();
            // levelService.OnProgressUpdate += UpdateLoadingScreen;
            loadingScreen.AnimateLoadingBar(5, LoadNextLevelAsync);
            // levelService.OnProgressUpdate -= UpdateLoadingScreen;
        }

        private async void LoadNextLevelAsync()
        {
            await levelService.LoadNextLevel();
            await screenNavigator.CloseScreen<AppLoadingScreen>();
        }


        private void UpdateLoadingScreen(float progress)
        {
            loadingScreen.SetProgress(progress);
        }
    }
}
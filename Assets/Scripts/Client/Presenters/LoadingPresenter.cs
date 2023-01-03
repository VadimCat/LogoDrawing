using Cysharp.Threading.Tasks;
using Ji2Core.Core.ScreenNavigation;
using Ji2Core.UI.Screens;
using UnityEngine;

namespace Client.Presenters
{
    //TODO: Unify to be available to move Ji2.CorePresenters
    public class LoadingPresenter
    {
        private readonly ScreenNavigator screenNavigator;
        private readonly LevelService levelService;
        private readonly float forceLoadingDuration;

        private LoadingScreen loadingScreen;

        public LoadingPresenter(ScreenNavigator screenNavigator, LevelService levelService)
        {
            this.screenNavigator = screenNavigator;
            this.levelService = levelService;
        }

        public LoadingPresenter(ScreenNavigator screenNavigator, LevelService levelService, float forceLoadingDuration) 
            : this(screenNavigator, levelService)
        {
            this.forceLoadingDuration = forceLoadingDuration;
        }

        public async UniTask LoadAsync()
        {
            if (Mathf.Approximately(0, forceLoadingDuration))
            {
                await Load();
            }
            else
            {
                await Load(forceLoadingDuration);
            }
        }
        
        private async UniTask Load(float forceLoadingDuration)
        {
            loadingScreen = await screenNavigator.PushScreen<LoadingScreen>();

            var fakeProgressTask = loadingScreen.AnimateLoadingBar(forceLoadingDuration);
            var nextLevel = await levelService.LoadNextLevelAsync();

            await fakeProgressTask;
            await screenNavigator.CloseScreen<LoadingScreen>();
            
            nextLevel.Start();
        }
        
        private async UniTask Load()
        {
            loadingScreen = await screenNavigator.PushScreen<LoadingScreen>();
            levelService.OnProgressUpdate += UpdateLoadingScreen;
            var level = await levelService.LoadNextLevelAsync();
            
            levelService.OnProgressUpdate -= UpdateLoadingScreen;

            await screenNavigator.CloseScreen<LoadingScreen>();
            level.Start();
        }
        
        private void UpdateLoadingScreen(float progress)
        {
            loadingScreen.SetProgress(progress);
        }
    }
}
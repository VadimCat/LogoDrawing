using System;
using Client.Screens;
using Cysharp.Threading.Tasks;
using Presenter;
using UnityEngine;

namespace Client
{
    public class LoadingPresenter
    {
        private readonly ScreenNavigator screenNavigator;
        private readonly LevelService levelService;
        private readonly float forceLoadingDuration;

        private AppLoadingScreen loadingScreen;

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
            loadingScreen = await screenNavigator.PushScreen<AppLoadingScreen>();

            var fakeProgressTask = loadingScreen.AnimateLoadingBar(forceLoadingDuration);
            var nextLevel = await levelService.LoadNextLevelAsync();

            await fakeProgressTask;
            await screenNavigator.CloseScreen<AppLoadingScreen>();
            
            nextLevel.Start();
        }
        
        private async UniTask Load()
        {
            loadingScreen = await screenNavigator.PushScreen<AppLoadingScreen>();
            levelService.OnProgressUpdate += UpdateLoadingScreen;
            var level = await levelService.LoadNextLevelAsync();
            
            levelService.OnProgressUpdate -= UpdateLoadingScreen;

            await screenNavigator.CloseScreen<AppLoadingScreen>();
            level.Start();
        }
        
        private void UpdateLoadingScreen(float progress)
        {
            loadingScreen.SetProgress(progress);
        }
    }

    public class LoadingPresenterFactory
    {
        private readonly ScreenNavigator screenNavigator;
        private readonly LevelService levelService;

        public LoadingPresenterFactory(ScreenNavigator screenNavigator, LevelService levelService)
        {
            this.screenNavigator = screenNavigator;
            this.levelService = levelService;
        }
        public LoadingPresenter Create(float forceLoadingDuration = 0)
        {
            if (Mathf.Approximately(forceLoadingDuration, 0))
            {
                return new LoadingPresenter(screenNavigator, levelService);
            }
            else
            {
                return new LoadingPresenter(screenNavigator, levelService, forceLoadingDuration);
            }
        }
    }
}
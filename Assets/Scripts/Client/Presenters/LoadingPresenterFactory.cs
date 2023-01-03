using Ji2Core.Core.ScreenNavigation;
using UnityEngine;

namespace Client.Presenters
{
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
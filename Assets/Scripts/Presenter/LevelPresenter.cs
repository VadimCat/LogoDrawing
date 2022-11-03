using Client.Screens;
using Cysharp.Threading.Tasks;
using Models;
using SceneView;
using Utils.Client;

namespace Presenter
{
    public class LevelPresenter
    {
        private readonly Level level;
        private readonly LevelViewData levelData;
        private readonly LevelService levelService;
        private readonly ScreenNavigator screenNavigator;
        private readonly LevelViewContainer view;

        public LevelPresenter(Level level, LevelViewContainer view, LevelViewData levelData, LevelService levelService,
            ScreenNavigator screenNavigator)
        {
            this.level = level;
            this.view = view;
            this.levelData = levelData;
            this.levelService = levelService;
            this.screenNavigator = screenNavigator;

            switch (level.Stage.Value)
            {
                case ColoringStage.Cleaning:
                    level.OnCleaningComplete += SetColoringStage;
                    SetCleaningStage();
                    break;
                case ColoringStage.Coloring:
                    SetColoringStageInstant();
                    break;
            }

            view.Progress.OnValueChanged += level.UpdateColoringProgress;

            level.OnColoringComplete += CompleteLevel;
        }

        private async void CompleteLevel()
        {
            levelService.Save();
            var screen = await screenNavigator.PushScreen<LevelCompletedScreen>();
            screen.OnClickNext += SwitchToNextLevel;
        }

        private void SwitchToNextLevel()
        {
            view.EnableColoring(false);
            screenNavigator.CloseScreen<LevelCompletedScreen>();
            levelService.LoadNextLevel();
        }

        private void SetCleaningStage()
        {
            view.SetColoringData(levelData.DirtView);
        }

        private void SetColoringStageInstant()
        {
            view.SetColoringData(levelData.ColoringView);
        }
        
        private async void SetColoringStage()
        {
            view.RemoveColoringObject();
            view.SetColoringData(levelData.ColoringView);

            view.EnableColoring(false);
            view.Progress.OnValueChanged -= level.UpdateColoringProgress;

            await UniTask.Delay(2000);

            view.Progress.OnValueChanged += level.UpdateColoringProgress;
            view.EnableColoring(true);
        }
    }
}
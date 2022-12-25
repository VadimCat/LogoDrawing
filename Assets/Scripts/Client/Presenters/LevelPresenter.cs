using System;
using Client.Audio;
using Client.Cursors;
using Client.Painting;
using Client.Screens;
using Client.UI.Screens;
using Cysharp.Threading.Tasks;
using Models;
using SceneView;
using UI;
using Utils.Client;

namespace Client.Presenters
{
    public class LevelPresenter
    {
        private const string levelNamePattern = "LEVEL {0}";
        private readonly Level level;
        private readonly LevelViewData levelData;
        private readonly Painter painter;
        private readonly LoadingPresenterFactory loadingPresenterFactory;
        private readonly ScreenNavigator screenNavigator;
        private readonly CursorService cursorService;
        private readonly LevelViewContainer view;
        private readonly ComplimentsWordsService complimentsWordsService;
        private readonly AudioService audioService;

        private ColoringLevelScreen levelScreen;

        public LevelPresenter(Level level, LevelViewContainer view, LevelViewData levelData, Painter painter,
            LoadingPresenterFactory loadingPresenterFactory, ScreenNavigator screenNavigator, CursorService cursorService,
            ComplimentsWordsService complimentsWordsService, AudioService audioService)
        {
            this.level = level;
            this.view = view;
            this.levelData = levelData;
            this.painter = painter;
            this.loadingPresenterFactory = loadingPresenterFactory;
            this.screenNavigator = screenNavigator;
            this.cursorService = cursorService;
            this.complimentsWordsService = complimentsWordsService;
            this.audioService = audioService;
        }

        private void LoadFromSave(Level level)
        {
            switch (level.Stage.Value)
            {
                case ColoringStage.Cleaning:
                    level.OnCleaningComplete += SetColoringStage;
                    SetCleaningStage();
                    break;
                case ColoringStage.Coloring:
                    SetColoringStageFromSave();
                    break;
            }
            view.EnableProgressUpdate(true);
        }

        public async void Start()
        {
            LoadFromSave(level);

            view.Progress.OnValueChanged += level.UpdateColoringProgress;

            level.OnColoringComplete += CompleteLevel;
            levelScreen = await screenNavigator.PushScreen<ColoringLevelScreen>();
            levelScreen.SetLevelName(string.Format(levelNamePattern, level.LevelPlayedTotal + 1));
            switch (level.Stage.Value)
            {
                case ColoringStage.Cleaning:
                    view.Progress.OnValueChanged += UpdateCleaningProgress;
                    break;
                case ColoringStage.Coloring:
                    view.Progress.OnValueChanged += UpdateColoringProgress;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void UpdateColoringProgress(float progress, float prevProgress)
        {
            levelScreen.SetColoringProgress(progress);
        }

        private void UpdateCleaningProgress(float progress, float prevProgress)
        {
            levelScreen.SetCleaningProgress(progress);
        }

        private async void CompleteLevel()
        {
            cursorService.DisableCurrent();
            view.gameObject.SetActive(false);
            view.Progress.OnValueChanged -= UpdateColoringProgress;

            audioService.PlaySfxAsync(AudioClipName.WinFX);
            
            var screen = await screenNavigator.PushScreen<LevelCompletedScreen>();
            screen.SetLevelResult(levelData.LevelResult, level.LevelPlayedTotal + 1);
            screen.OnClickNext += SwitchToNextLevel;
        }

        private async void SwitchToNextLevel()
        {
            audioService.PlaySfxAsync(AudioClipName.ButtonFX);
            view.EnableProgressUpdate(false);
            await screenNavigator.CloseScreen<LevelCompletedScreen>();
            await loadingPresenterFactory.Create(1f).LoadAsync();
        }

        private void SetCleaningStage()
        {
            view.EnableProgressUpdate(true);

            cursorService.SetCleaning();
            view.SetColoringData(levelData.DirtView);
        }

        private void SetColoringStageFromSave()
        {
            cursorService.SetColoring();
            view.SetColoringData(levelData.ColoringView);
        }

        private async void SetColoringStage()
        {
            levelScreen.PlayCleaningCompleteVfx();
            complimentsWordsService.ShowRandomFromScreenPosition(cursorService.PointerScreenPosition);

            view.EnableProgressUpdate(false);
            view.Progress.OnValueChanged -= level.UpdateColoringProgress;
            view.Progress.OnValueChanged -= UpdateCleaningProgress;
            
            view.RemoveColoringObject();
            SetColoringStageFromSave();
            await UniTask.Delay(2000);
            
            view.Progress.OnValueChanged += UpdateColoringProgress;
            view.Progress.OnValueChanged += level.UpdateColoringProgress;
            
            view.EnableProgressUpdate(true);
        }
    }
}
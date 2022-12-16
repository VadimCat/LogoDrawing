using System;
using Client;
using Client.Audio;
using Client.Screens;
using Cysharp.Threading.Tasks;
using Models;
using SceneView;
using UI;
using UnityEngine;
using Utils.Client;

namespace Presenter
{
    public class LevelPresenter
    {
        private const string levelNamePattern = "LEVEL {0}";
        private readonly Level level;
        private readonly LevelViewData levelData;
        private readonly LevelService levelService;
        private readonly ScreenNavigator screenNavigator;
        private readonly CursorService cursorService;
        private readonly LevelViewContainer view;
        private readonly ComplimentsWordsService complimentsWordsService;
        private readonly AudioService audioService;

        private ColoringLevelScreen levelScreen;
        
        public LevelPresenter(Level level, LevelViewContainer view, LevelViewData levelData, LevelService levelService,
            ScreenNavigator screenNavigator, CursorService cursorService, ComplimentsWordsService complimentsWordsService,
            AudioService audioService)
        {
            this.level = level;
            this.view = view;
            this.levelData = levelData;
            this.levelService = levelService;
            this.screenNavigator = screenNavigator;
            this.cursorService = cursorService;
            this.complimentsWordsService = complimentsWordsService;
            this.audioService = audioService;

            LoadFromSave(level);

            view.Progress.OnValueChanged += level.UpdateColoringProgress;

            level.OnColoringComplete += CompleteLevel;
            StartLevel();
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
                    SetColoringStageInstant();
                    break;
            }
        }

        private async void StartLevel()
        {
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
            cursorService.Disable();
            view.gameObject.SetActive(false);
            view.Progress.OnValueChanged -= UpdateColoringProgress;

            levelService.Save();
            audioService.PlaySfxAsync(AudioClipName.WinFX);
            var screen = await screenNavigator.PushScreen<LevelCompletedScreen>();
            screen.SetLevelResult(levelData.LevelResult, level.LevelPlayedTotal + 1);
            screen.OnClickNext += SwitchToNextLevel;
        }

        private async void SwitchToNextLevel()
        {
            audioService.PlaySfxAsync(AudioClipName.ButtonFX);
            view.EnableColoring(false);
            await screenNavigator.CloseScreen<LevelCompletedScreen>();
            await levelService.LoadNextLevel();
        }

        private void SetCleaningStage()
        {
            cursorService.SetCleaning();
            view.SetColoringData(levelData.DirtView);
        }

        private void SetColoringStageInstant()
        {
            complimentsWordsService.ShowRandomComplimentWordFromScreenPosition(cursorService.PointerScreenPosition);
            cursorService.SetColoring();
            view.SetColoringData(levelData.ColoringView);
        }
        
        private async void SetColoringStage()
        {
            view.RemoveColoringObject();
            SetColoringStageInstant();

            view.EnableColoring(false);
            view.Progress.OnValueChanged -= level.UpdateColoringProgress;
            view.Progress.OnValueChanged -= UpdateCleaningProgress;
            view.Progress.OnValueChanged += UpdateColoringProgress;

            await UniTask.Delay(2000);

            view.Progress.OnValueChanged += level.UpdateColoringProgress;
            view.EnableColoring(true);
        }
    }
}
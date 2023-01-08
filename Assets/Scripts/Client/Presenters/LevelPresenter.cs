using System;
using Client.Cursors;
using Client.Painting;
using Client.UI.Screens;
using Cysharp.Threading.Tasks;
using Ji2Core.Core;
using Ji2Core.Core.Compliments;
using Ji2Core.Core.ScreenNavigation;
using Ji2Core.Core.Audio;
using Ji2Core.Plugins.AppMetrica;
using Models;
using SceneView;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Client.Presenters
{
    public class LevelPresenter : IUpdatable
    {
        private const string LEVEL_NAME_PATTERN = "LEVEL {0}";
        private readonly Level level;
        private readonly LevelViewData levelData;
        private readonly LoadingPresenterFactory loadingPresenterFactory;
        private readonly ScreenNavigator screenNavigator;
        private readonly CursorService cursorService;
        private readonly LevelViewContainer view;
        private readonly ComplimentsWordsService complimentsWordsService;
        private readonly AudioService audioService;
        private readonly UpdateService updateService;

        private ColoringLevelScreen levelScreen;

        public LevelPresenter(Level level, LevelViewContainer view, LevelViewData levelData,
            LoadingPresenterFactory loadingPresenterFactory, ScreenNavigator screenNavigator,
            CursorService cursorService, ComplimentsWordsService complimentsWordsService, AudioService audioService,
            UpdateService updateService)
        {
            this.level = level;
            this.view = view;
            this.levelData = levelData;
            this.loadingPresenterFactory = loadingPresenterFactory;
            this.screenNavigator = screenNavigator;
            this.cursorService = cursorService;
            this.complimentsWordsService = complimentsWordsService;
            this.audioService = audioService;
            this.updateService = updateService;
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
            levelScreen.SetLevelName(string.Format(LEVEL_NAME_PATTERN, level.LevelPlayedTotal + 1));
            switch (level.Stage.Value)
            {
                case ColoringStage.Cleaning:
                    view.Progress.OnValueChanged += UpdateCleaningProgress;
                    break;
                case ColoringStage.Coloring:
                    SetCleaningProgress(1f);
                    view.Progress.OnValueChanged += UpdateColoringProgress;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            level.LogAnalyticsLevelStart();
            updateService.Add(this);
        }

        public void OnUpdate()
        {
            level.AppendPlayTime(Time.deltaTime);
        }

        private void UpdateColoringProgress(float progress, float prevProgress)
        {
            levelScreen.UpdateColoringProgress(progress);
        }

        private void UpdateCleaningProgress(float progress, float prevProgress)
        {
            levelScreen.UpdateCleaningProgress(progress);
        }

        private void SetCleaningProgress(float progress)
        {
            levelScreen.SetCleaningProgress(progress);
        }

        private async void CompleteLevel()
        {
            updateService.Remove(this);
            level.LogAnalyticsLevelFinish();

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
            complimentsWordsService.ShowRandomFromScreenPosition(complimentsWordsService.transform.position +
                                                                 Vector3.one * Random.Range(-200, 200));

            view.EnableProgressUpdate(false);
            view.Progress.OnValueChanged -= level.UpdateColoringProgress;
            view.Progress.OnValueChanged -= UpdateCleaningProgress;
            levelScreen.ShowNextButton();
            cursorService.DisableCurrent();
            levelScreen.OnClickNext += SetColoringStageAfterClickNextButton;
        }

        private async void SetColoringStageAfterClickNextButton()
        {
            levelScreen.HideNextButton();
            view.RemoveColoringObject();
            SetColoringStageFromSave();
            await UniTask.Delay(2000);

            view.Progress.OnValueChanged += UpdateColoringProgress;
            view.Progress.OnValueChanged += level.UpdateColoringProgress;

            view.EnableProgressUpdate(true);
        }
    }
}
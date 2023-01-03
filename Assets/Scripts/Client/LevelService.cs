using System;
using Client.Cursors;
using Client.Painting;
using Client.Presenters;
using Client.UI.Screens;
using Cysharp.Threading.Tasks;
using Ji2Core.Core;
using Ji2Core.Core.Compliments;
using Ji2Core.Core.ScreenNavigation;
using Ji2Core.Core.Audio;
using Ji2Core.Models;
using Models;
using SceneView;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Client
{
    public class LevelService : ISavable, IUpdatable
    {
        public event Action<float> OnProgressUpdate;

        private const string SAVE_KEY = "lvl";

        private int currentLvlInd = -1;

        private readonly LevelsViewDataStorage levelsViewDataStorage;
        private readonly LevelViewContainer levelView;
        private readonly ScreenNavigator screenNavigator;
        private readonly UpdateService updateService;
        private readonly SceneLoader sceneLoader;
        private readonly BackgroundService backgroundService;
        private readonly Context context;

        private AsyncOperation currentLoadingTask;

        //TODO: Check dependencies and use context where needed
        public LevelService(LevelsViewDataStorage levelsViewDataStorage, LevelViewContainer levelView,
            ScreenNavigator screenNavigator, UpdateService updateService, BackgroundService backgroundService,
            Context context, SceneLoader sceneLoader)
        {
            this.levelsViewDataStorage = levelsViewDataStorage;
            this.levelView = levelView;
            this.screenNavigator = screenNavigator;
            this.updateService = updateService;
            this.sceneLoader = sceneLoader;
            this.backgroundService = backgroundService;
            this.context = context;

            Load();
        }

        public async UniTask<LevelPresenter> LoadNextLevelAsync()
        {
            currentLvlInd++;
            int lvlToLoad = GetNormalizedLevelInd(currentLvlInd);

            var sceneLoadingTask = sceneLoader.LoadScene("LevelScene");
            await UniTask.WhenAll(sceneLoadingTask, UniTask.Delay(2000));

            backgroundService.SwitchBackground(BackgroundService.Background.Game);

            var viewData = levelsViewDataStorage.GetData(levelsViewDataStorage.levelsList[lvlToLoad]);
            var view = Object.Instantiate(levelView);
            view.SetDependencies(updateService);
            
            //TODO: Level presenter should create or get level from factory?
            
            var level = new Level(viewData.ID, currentLvlInd);
            level.OnColoringComplete += Save;
            
            return new LevelPresenter(level, view, viewData, context.GetService<Painter>(),
                context.GetService<LoadingPresenterFactory>(), screenNavigator,
                context.GetService<CursorService>(), context.GetService<ComplimentsWordsService>(),
                context.GetService<AudioService>());
        }

        public void OnUpdate()
        {
            OnProgressUpdate?.Invoke(currentLoadingTask.progress);
        }

        public void Save()
        {
            PlayerPrefs.SetInt(SAVE_KEY, currentLvlInd);
        }

        public void Load()
        {
            if (PlayerPrefs.HasKey(SAVE_KEY))
                currentLvlInd = PlayerPrefs.GetInt(SAVE_KEY);
        }

        public void ClearSave()
        {
            PlayerPrefs.DeleteKey(SAVE_KEY);
        }

        private int GetNormalizedLevelInd(int lvlInd)
        {
            while (lvlInd >= levelsViewDataStorage.levelsList.Count)
            {
                lvlInd -= levelsViewDataStorage.levelsList.Count;
            }

            return lvlInd;
        }
    }
}
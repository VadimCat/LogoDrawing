using System;
using Client.Screens;
using SceneView;
using UI;
using UnityEngine;
using Utils.Client;

namespace Client
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private LevelsViewDataStorage levelsStorage;
        [SerializeField] private LevelViewContainer levelViewOrigin;
        [SerializeField] private ScreenNavigator screenNavigator;
        [SerializeField] private BackgroundService backgroundService;
        [SerializeField] private UpdateService updateService;
        [SerializeField] private CursorService cursorService;
        [SerializeField] private ComplimentsWordsService complimentsWordsService;

        private readonly Context context = new();
        private LevelService levelService;

        private async void Awake()
        {
            DontDestroyOnLoad(this);
            InstallLevelsData();
            InstallNavigator();
            InstallСursor();
            InstallComplimentsWords();

            levelService = new LevelService(levelsStorage, levelViewOrigin, screenNavigator, updateService,
                backgroundService, context);

            var loadingFactory = new LoadingPresenterFactory(screenNavigator, levelService);
            
            context.Register(loadingFactory);
            context.Register(cursorService);
            context.Register(levelService);
            context.Register(complimentsWordsService);

            await loadingFactory.Create(5).LoadAsync();
        }

        private void InstallСursor()
        {
            cursorService.Construct(updateService);
        }

        private void InstallNavigator()
        {
            screenNavigator.Bootstrap();
        }

        private void InstallLevelsData()
        {
            levelsStorage.Bootstrap();
        }

        private void InstallComplimentsWords()
        {
            complimentsWordsService.Bootstrap();
        }
    }
}
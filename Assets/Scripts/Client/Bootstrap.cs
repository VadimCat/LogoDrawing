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
        [SerializeField] private BackGroundService backGroundService;
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

            levelService = new LevelService(levelsStorage, levelViewOrigin, screenNavigator, updateService,
                backGroundService, context);

            new LoadingPresenter(screenNavigator, backGroundService, levelService).Load();

            context.Register(cursorService);
            context.Register(levelService);
            context.Register(complimentsWordsService);
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
    }
}
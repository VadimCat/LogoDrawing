using Client.Screens;
using Cysharp.Threading.Tasks;
using Presenter;
using SceneView;
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
        
        private readonly Context context = new();
        private LevelService levelService;
        
        private async void Awake()
        {
            DontDestroyOnLoad(this);
            InstallLevelsData();
            InstallNavigator();

            levelService = new LevelService(levelsStorage, levelViewOrigin, screenNavigator, updateService, backGroundService);

            new LoadingPresenter(screenNavigator, backGroundService, levelService).Load();
            
            context.Register(levelService);
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

    public class LoadingPresenter
    {
        private readonly ScreenNavigator screenNavigator;
        private readonly BackGroundService backGroundService;
        private readonly LevelService levelService;

        private AppLoadingScreen loadingScreen;

        public LoadingPresenter(ScreenNavigator screenNavigator, BackGroundService backGroundService,
            LevelService levelService)
        {
            this.screenNavigator = screenNavigator;
            this.backGroundService = backGroundService;
            this.levelService = levelService;
        }

        public async UniTask Load()
        {
            backGroundService.SwitchBackground(Background.Loading);
            loadingScreen = await screenNavigator.PushScreen<AppLoadingScreen>();
            levelService.OnProgressUpdate += UpdateLoadingScreen;

            await UniTask.WhenAll(levelService.LoadNextLevel(), UniTask.Delay(2000)) ;
            await screenNavigator.CloseScreen<AppLoadingScreen>();
            levelService.OnProgressUpdate -= UpdateLoadingScreen;
        }
        
        private void UpdateLoadingScreen(float progress)
        {
            loadingScreen.SetProgress(progress);
        }
    }
}

using Client.Screens;
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
}

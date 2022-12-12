using Client.Audio;
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
        [SerializeField] private AudioService audioService;
        
        
        private readonly Context context = new();
        private LevelService levelService;

        private void Awake()
        {
            DontDestroyOnLoad(this);
            InstallLevelsData();
            InstallNavigator();
            InstallСursor();
            InstallComplimentsWordsShowData();
            InstallAudioService();

            audioService.PlayMusic(AudioClipName.DefaultBackgroundMusic);
            
            levelService = new LevelService(levelsStorage, levelViewOrigin, screenNavigator, updateService,
                backGroundService, context);

            new LoadingPresenter(screenNavigator, backGroundService, levelService).Load();

            context.Register(cursorService);
            context.Register(levelService);
            context.Register(complimentsWordsService);
        }

        private void InstallAudioService()
        {
            audioService.Bootstrap();
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

        private void InstallComplimentsWordsShowData()
        {
            complimentsWordsService.Bootstrap();
        }
    }
}
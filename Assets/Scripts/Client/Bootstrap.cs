using Client.Audio;
using Client.Audio.SfxPlayers;
using Client.Painting;
using Client.Pools;
using Client.Screens;
using Core;
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
        [SerializeField] private AudioService audioService;
        [Header("Installers")]
        [SerializeField] private PainterInstaller painterInstaller;
        
        private Pool<SfxPlaybackSource> sfxPlaybackPool;

        private readonly Context context = new();
        private LevelService levelService;

        private async void Start()
        {
            DontDestroyOnLoad(this);
            InstallAudioService();
            InstallLevelsData();
            InstallNavigator();

            context.Register(new InputService(updateService));
            InstallСursor();

            audioService.PlayMusic(AudioClipName.DefaultBackgroundMusic);

            levelService = new LevelService(levelsStorage, levelViewOrigin, screenNavigator, updateService,
                backgroundService, context);
            var loadingFactory = new LoadingPresenterFactory(screenNavigator, levelService);

            context.Register(audioService);
            context.Register(cursorService);
            context.Register(levelService);
            context.Register(complimentsWordsService);
            context.Register(loadingFactory);

            await loadingFactory.Create(5).LoadAsync();
        }

        private void InstallAudioService()
        {
            audioService.Bootstrap();
        }

        private void InstallСursor()
        {
            cursorService.SetDependencies(context.GetService<InputService>(), audioService);
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
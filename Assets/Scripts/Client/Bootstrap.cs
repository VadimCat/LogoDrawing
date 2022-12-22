using Client.Audio;
using Client.Audio.SfxPlayers;
using Client.Cursors;
using Client.Painting;
using Client.Pools;
using Client.Screens;
using Core;
using Core.CameraProvider;
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
            //TODO: Create installers where needed
            InstallCamera();
            InstallAudioService();
            InstallLevelsData();
            InstallNavigator();
            InstallInputService();
            InstallСursor();
            
            painterInstaller.Install(context);
            
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

        private void InstallCamera()
        {
            context.Register(new CameraProvider());
        }

        private void InstallInputService()
        {
            context.Register(new InputService(updateService, context.GetService<CameraProvider>()));
        }

        private void InstallAudioService()
        {
            audioService.Bootstrap();
            audioService.PlayMusic(AudioClipName.DefaultBackgroundMusic);
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
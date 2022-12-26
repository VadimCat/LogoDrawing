using Client.Audio;
using Client.Audio.SfxPlayers;
using Client.Cursors;
using Client.Painting;
using Client.Pools;
using Client.Screens;
using Client.UI;
using Client.UI.Screens;
using Core;
using Core.Camera;
using Core.UserInput;
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
        [SerializeField] private BackgroundService backgroundService;
        [SerializeField] private UpdateService updateService;
        [SerializeField] private CursorService cursorService;
        [SerializeField] private ComplimentsWordsService complimentsWordsService;
        [SerializeField] private AudioService audioService;
        [SerializeField] private JoystickInputConfig joystickInputConfig;

        [Header("Installers")]
        [SerializeField] private PainterInstaller painterInstaller;

        [SerializeField] private JoystickInstaller joystickInstaller;

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
            context.Register(updateService);
            painterInstaller.Install(context);
            joystickInstaller.Install(context);
            InstallСursor();

            levelService = new LevelService(levelsStorage, levelViewOrigin, screenNavigator, updateService,
                backgroundService, context);
            var loadingFactory = new LoadingPresenterFactory(screenNavigator, levelService);

            context.Register(audioService);
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
            context.Register(new InputService(updateService));
        }

        private void InstallAudioService()
        {
            audioService.Bootstrap();
            audioService.PlayMusic(AudioClipName.DefaultBackgroundMusic);
        }

        private void InstallСursor()
        {
            var factory = new CursorInputHandlerFactory(context, joystickInputConfig);

            cursorService.SetDependencies(context.GetService<InputService>(), audioService,
                context.GetService<Painter>(), context.GetService<CameraProvider>(),
                context.GetService<Joystick>(), factory);
            context.Register(cursorService);
            cursorService.Bootstrap();
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
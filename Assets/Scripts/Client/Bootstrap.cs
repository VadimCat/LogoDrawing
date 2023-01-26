using Client.Cursors;
using Client.Painting;
using Client.Presenters;
using Client.States;
using Client.UI.Screens;
using Client.View;
using Ji2Core.Core;
using Ji2Core.Core.Analytics;
using Ji2Core.Core.Audio;
using Ji2Core.Core.Compliments;
using Ji2Core.Core.ScreenNavigation;
using Ji2Core.Core.States;
using Ji2Core.Core.UserInput;
using Ji2Core.Plugins.AppMetrica;
using Models;
using UI.Background;
using UnityEngine;

namespace Client
{
    public class Bootstrap : BootstraperBase
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
        [SerializeField] private DirectCursorInputHandlerConfig directCursorInputHandlerConfig;

        [Header("Installers")]
        [SerializeField] private PainterInstaller painterInstaller;

        [SerializeField] private JoystickInstaller joystickInstaller;

        private AppSession appSession; 
            
        private readonly Context context = Context.GetInstance();

        protected override void Start()
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

            var sceneLoader = new SceneLoader(updateService);
            
            var analytics = new Analytics(); 
            analytics.AddLogger(new YandexMetricaLogger(AppMetrica.Instance));
            
            var levelService = new LevelService(levelsStorage, levelViewOrigin, screenNavigator, updateService,
                backgroundService, context, sceneLoader, analytics);
            var loadingFactory = new LoadingPresenterFactory(screenNavigator, levelService);

            context.Register(sceneLoader);
            context.Register(audioService);
            context.Register(levelService);
            context.Register(complimentsWordsService);
            context.Register(loadingFactory);
            
            StateMachine appStateMachine = new StateMachine(new StateFactory(context));
            
            appSession = new AppSession(appStateMachine);
            
            appSession.StateMachine.Enter<InitialState>();
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
            var factory = new CursorInputHandlerFactory(context, joystickInputConfig, directCursorInputHandlerConfig);

            cursorService.SetDependencies(context.GetService<InputService>(), audioService,
                context.GetService<Painter>(), factory);
            context.Register(cursorService);
            cursorService.Bootstrap();
        }

        private void InstallNavigator()
        {
            screenNavigator.Bootstrap();
            context.Register(screenNavigator);
        }

        private void InstallLevelsData()
        {
            levelsStorage.Bootstrap();
        }
    }
}
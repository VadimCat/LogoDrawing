using Cysharp.Threading.Tasks;
using Facebook.Unity;
using Ji2Core.Core;
using Ji2Core.Core.ScreenNavigation;
using Ji2Core.Core.States;
using Ji2Core.Plugins.AppMetrica;
using Ji2Core.UI.Screens;

namespace Client.States
{
    public class InitialState : IState
    {
        private const string GameSceneName = "LevelScene";
        private readonly StateMachine _stateMachine;
        private readonly ScreenNavigator _screenNavigator;
        private readonly SceneLoader _sceneLoader;

        private LoadingScreen _loadingScreen;
    
        public InitialState(StateMachine stateMachine, ScreenNavigator screenNavigator, SceneLoader sceneLoader)
        {
            _stateMachine = stateMachine;
            _screenNavigator = screenNavigator;
            _sceneLoader = sceneLoader;
        }

        public async UniTask Exit()
        {
            await _screenNavigator.CloseScreen<LoadingScreen>();
        }

        public async UniTask Enter()
        {
            var loadingTask = _sceneLoader.LoadScene(GameSceneName);
            var facebookTask = LoadFB();
            _loadingScreen = await _screenNavigator.PushScreen<LoadingScreen>();
        
            _sceneLoader.OnProgressUpdate += UpdateProgress;
            await UniTask.WhenAll(loadingTask, facebookTask);
            
            
            _sceneLoader.OnProgressUpdate -= UpdateProgress;
        
            await _screenNavigator.CloseScreen<LoadingScreen>();

            _loadingScreen = null;

            _stateMachine.Enter<GameState>();
        }

        private async UniTask LoadFB()
        {
            var taskCompletionSource = new UniTaskCompletionSource<bool>();
            FB.Init(() => OnFbInitComplete(taskCompletionSource));
            
            await taskCompletionSource.Task;
            if(FB.IsInitialized)
                FB.ActivateApp();
        }

        private void OnFbInitComplete(UniTaskCompletionSource<bool> uniTaskCompletionSource)
        {
            uniTaskCompletionSource.TrySetResult(FB.IsInitialized);
        }

        private void UpdateProgress(float progress)
        {
            _loadingScreen.SetProgress(progress);
        }
    }

    public class LoadingSceneState : IState
    {
        public UniTask Enter()
        {
            throw new System.NotImplementedException();
        }

        public UniTask Exit()
        {
            throw new System.NotImplementedException();
        }
    }
}
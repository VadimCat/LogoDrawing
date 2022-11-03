using System.Threading.Tasks;
using Client.Screens;
using Cysharp.Threading.Tasks;
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

        private Context context = new();
        private LevelService levelService;
        private async void Awake()
        {
            DontDestroyOnLoad(this);
            InstallLevelsData();
            InstallNavigator();

            levelService = new LevelService(levelsStorage, levelViewOrigin, screenNavigator);
        
            context.Register(levelService);

            await levelService.LoadNextLevel();
        }

        private void InstallNavigator()
        {
            screenNavigator.Bootstrap();
        }

        private async Task IncLevel()
        {
            var level = await levelService.LoadNextLevel();
            // level.OnColoringComplete += LoadLevel;
        }

        private void InstallLevelsData()
        {
            levelsStorage.Bootstrap();
        }
    }
}

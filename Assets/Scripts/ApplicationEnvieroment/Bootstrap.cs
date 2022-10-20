using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using SceneView;
using UnityEngine;
using Utils.Client;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private LevelsViewDataStorage levelsStorage;
    [SerializeField] private LevelViewContainer levelViewOrigin;
    
    private LevelService levelService;
    private async void Awake()
    {
        DontDestroyOnLoad(this);
        InstallLevelsData();

        levelService = new LevelService(levelsStorage, levelViewOrigin);
        
        await IncLevel();
    }

    private async Task IncLevel()
    {
        var level = await levelService.LoadNextLevel();
        level.OnColoringComplete += LoadLevel;
    }

    private async void LoadLevel()
    {
        await UniTask.Delay(3000);
        await IncLevel();
    }

    private void InstallLevelsData()
    {
        levelsStorage.Bootstrap();
    }
}

using System;
using Client;
using Client.Audio;
using Client.Presenters;
using Client.Screens;
using Cysharp.Threading.Tasks;
using Models;
using SceneView;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.Client;
using Object = UnityEngine.Object;

public class LevelService : ISavable, IUpdatable
{
    public event Action<float> OnProgressUpdate;

    private const string Savekey = "lvl";

    private int currentLvlInd = -1;

    private readonly LevelsViewDataStorage levelsViewDataStorage;
    private readonly LevelViewContainer levelView;
    private readonly ScreenNavigator screenNavigator;
    private readonly UpdateService updateService;
    private readonly BackgroundService backgroundService;
    private readonly Context context;

    private AsyncOperation currentLoadingTask;

    //TODO: Check dependencies and use context where needed
    public LevelService(LevelsViewDataStorage levelsViewDataStorage, LevelViewContainer levelView,
        ScreenNavigator screenNavigator, UpdateService updateService, BackgroundService backgroundService,
        Context context)
    {
        this.levelsViewDataStorage = levelsViewDataStorage;
        this.levelView = levelView;
        this.screenNavigator = screenNavigator;
        this.updateService = updateService;
        this.backgroundService = backgroundService;
        this.context = context;

        Load();
    }

    public void OnUpdate()
    {
        OnProgressUpdate?.Invoke(currentLoadingTask.progress);
    }

    public async UniTask<LevelPresenter> LoadNextLevelAsync()
    {
        currentLvlInd++;
        int lvlToLoad = GetNormalizedLevelInd(currentLvlInd);

        currentLoadingTask = SceneManager.LoadSceneAsync("LevelScene", LoadSceneMode.Single);
        updateService.Add(this);

        await UniTask.WhenAll(currentLoadingTask.ToUniTask(), UniTask.Delay(2000));
        ForceFullProgress();
        updateService.Remove(this);

        backgroundService.SwitchBackground(Background.Game);
        var scene = SceneManager.GetSceneByName("LevelScene");
        SceneManager.SetActiveScene(scene);
        var viewData = levelsViewDataStorage.GetData(levelsViewDataStorage.levelsList[lvlToLoad]);
        var view = Object.Instantiate(levelView);
        view.SetDependencies(updateService);
        //TODO: Level presenter should create or get level from factory
        var level = new Level(viewData.ID, currentLvlInd);
        level.OnColoringComplete += Save;
        return new LevelPresenter(level, view, viewData, context.GetService<LoadingPresenterFactory>(), screenNavigator,
            context.GetService<CursorService>(), context.GetService<ComplimentsWordsService>(),
            context.GetService<AudioService>());
    }

    private int GetNormalizedLevelInd(int lvlInd)
    {
        while (lvlInd >= levelsViewDataStorage.levelsList.Count)
        {
            lvlInd -= levelsViewDataStorage.levelsList.Count;
        }

        return lvlInd;
    }

    private void ForceFullProgress()
    {
        OnProgressUpdate?.Invoke(1);
    }

    public void Save()
    {
        PlayerPrefs.SetInt(Savekey, currentLvlInd);
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey(Savekey))
            currentLvlInd = PlayerPrefs.GetInt(Savekey);
    }

    public void ClearSave()
    {
        PlayerPrefs.DeleteKey(Savekey);
    }
}
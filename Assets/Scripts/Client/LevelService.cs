using System;
using Client;
using Client.Screens;
using Cysharp.Threading.Tasks;
using Models;
using Presenter;
using SceneView;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.Client;
using Object = UnityEngine.Object;

public class LevelService : ISavable, IUpdatable
{
    public event Action<float> OnProgressUpdate;

    private const string Savekey = "lvl";

    private int currentLvlInd = -1;
    private string currentLvlId => levelsViewDataStorage.levelsList[currentLvlInd];

    private readonly LevelsViewDataStorage levelsViewDataStorage;
    private readonly LevelViewContainer levelView;
    private readonly ScreenNavigator screenNavigator;
    private readonly UpdateService updateService;
    private readonly BackGroundService backGroundService;
    private readonly Context context;

    private AsyncOperation currentLoadingTask;

    //TODO: Check dependencies and use context where needed
    public LevelService(LevelsViewDataStorage levelsViewDataStorage, LevelViewContainer levelView,
        ScreenNavigator screenNavigator, UpdateService updateService, BackGroundService backGroundService,
        Context context)
    {
        this.levelsViewDataStorage = levelsViewDataStorage;
        this.levelView = levelView;
        this.screenNavigator = screenNavigator;
        this.updateService = updateService;
        this.backGroundService = backGroundService;
        this.context = context;

        Load();
    }

    public void OnUpdate()
    {
        OnProgressUpdate?.Invoke(currentLoadingTask.progress);
    }

    public async UniTask<Level> LoadNextLevel()
    {
        currentLvlInd++;
        if (currentLvlInd >= levelsViewDataStorage.levelsList.Count)
            currentLvlInd = 0;


        currentLoadingTask = SceneManager.LoadSceneAsync("LevelScene", LoadSceneMode.Single);
        updateService.Add(this);

        await UniTask.WhenAll(currentLoadingTask.ToUniTask(), UniTask.Delay(2000));
        ForceFullProgress();
        updateService.Remove(this);

        backGroundService.SwitchBackground(Background.Game);
        var scene = SceneManager.GetSceneByName("LevelScene");
        SceneManager.SetActiveScene(scene);
        var viewData = levelsViewDataStorage.GetData(currentLvlId);
        var view = Object.Instantiate(levelView);
        var level = new Level(viewData.ID);
        new LevelPresenter(level, view, viewData, this, screenNavigator, context.GetService<CursorService>());
        return level;
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

public interface ISavable
{
    public void Save();
    public void Load();
    public void ClearSave();
}
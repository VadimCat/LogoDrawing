using System.Threading.Tasks;
using Client.Screens;
using Cysharp.Threading.Tasks;
using Models;
using Presenter;
using SceneView;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.Client;

public class LevelService : ISavable
{
    private const string Savekey = "lvl";
    
    private int currentLvlInd = -1;
    private string currentLvlId => levelsViewDataStorage.levelsList[currentLvlInd];

    private readonly LevelsViewDataStorage levelsViewDataStorage;
    private readonly LevelViewContainer levelView;
    private readonly ScreenNavigator screenNavigator;

    public LevelService(LevelsViewDataStorage levelsViewDataStorage, LevelViewContainer levelView,
        ScreenNavigator screenNavigator)
    {
        this.levelsViewDataStorage = levelsViewDataStorage;
        this.levelView = levelView;
        this.screenNavigator = screenNavigator;
        
        Load();    
    }

    public async Task<Level> LoadNextLevel()
    {
        currentLvlInd++;
        if (currentLvlInd >= levelsViewDataStorage.levelsList.Count)
            currentLvlInd = 0;
        
        await SceneManager.LoadSceneAsync("LevelScene", LoadSceneMode.Single).ToUniTask();
        var scene = SceneManager.GetSceneByName("LevelScene");
        SceneManager.SetActiveScene(scene);
        var viewData = levelsViewDataStorage.GetData(currentLvlId);
        var view = Object.Instantiate(levelView);
        var level = new Level("bmw");
        new LevelPresenter(level, view, viewData, this, screenNavigator);
        return level;
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
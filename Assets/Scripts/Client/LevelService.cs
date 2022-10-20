using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Presenter;
using SceneView;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.Client;

public class LevelService
{
    private int currentLvlInd = -1;
    private string currentLvlId => levelsViewDataStorage.levelsList[currentLvlInd];
    
    private readonly LevelsViewDataStorage levelsViewDataStorage;
    private readonly LevelViewContainer levelView;

    public LevelService(LevelsViewDataStorage levelsViewDataStorage, LevelViewContainer levelView)
    {
        this.levelsViewDataStorage = levelsViewDataStorage;
        this.levelView = levelView;
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
        new LevelPresenter(level, view, viewData);
        return level;
    }
}

using SceneView;
using UnityEngine;
using Utils.Client;

namespace Presenter
{
    public class LevelPresenter
    {
        private readonly Level level;
        private readonly LevelViewData levelData;
        private readonly LevelViewContainer view;
        
        public LevelPresenter(Level level, LevelViewContainer view, LevelViewData levelData)
        {
            this.level = level;
            this.view = view;
            this.levelData = levelData;

            SetCleaningStage();

            view.Progress.OnValueChanged += level.UpdateColoringProgress;
            level.OnCleaningComplete += SetColoringStage;
        }

        private void SetCleaningStage()
        {
            view.SetColoringData(levelData.DirtView);
        }

        private void SetColoringStage()
        {
            view.RemoveColoringObject();
            view.SetColoringData(levelData.ColoringView);
        }
    }
}
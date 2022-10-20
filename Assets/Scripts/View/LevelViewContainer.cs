using UnityEngine;
using Utils;

namespace SceneView
{
    public class LevelViewContainer : MonoBehaviour
    {
        private ColoringLevelView levelView;
        public ReactiveProperty<float> Progress => progress;

        private readonly ReactiveProperty<float> progress = new();

        public void SetColoringData(ColoringLevelView dirtView)
        {
            levelView = Instantiate(dirtView, transform);
        }

        public void RemoveColoringObject()
        {
            Destroy(levelView.gameObject);
        }
        
        private void Update()
        {
            if(Mathf.Approximately(1, levelView.ProgressRatio))
                return;

            progress.Value = levelView.ProgressRatio;
        }
    }
}
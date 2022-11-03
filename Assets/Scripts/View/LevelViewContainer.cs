using PaintIn3D;
using UnityEngine;
using Utils;

namespace SceneView
{
    public class LevelViewContainer : MonoBehaviour
    {
        [SerializeField] private P3dHitScreen hitScreen;

        private ColoringLevelView levelView;
        public ReactiveProperty<float> Progress => progress;

        private readonly ReactiveProperty<float> progress = new();

        public void SetColoringData(ColoringLevelView dirtView)
        {
            levelView = Instantiate(dirtView, transform);
        }

        public void EnableColoring(bool enable)
        {
            hitScreen.enabled = enable;
        }

        public void RemoveColoringObject()
        {
            Destroy(levelView.gameObject);
        }

        private void Update()
        {
            //HACK: TO FIX INNER BUGGY IMPLEMENTATION OF Coloring plugin
            if (Mathf.Approximately(1, levelView.ProgressRatio))
                return;

            progress.Value = levelView.ProgressRatio;
        }
    }
}
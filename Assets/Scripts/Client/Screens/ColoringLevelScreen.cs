using TMPro;
using UnityEngine;

namespace Client.Screens
{
    public class ColoringLevelScreen : BaseScreen
    {
        [SerializeField] private TextLoadingBar loadingBar;
        [SerializeField] private TMP_Text levelName;

        public void SetLevelName(string name)
        {
            levelName.text = name;
        }

        //TODO: Fix and create common class with ApploadingScreen loading functionality 
        public void SetCleaningProgress(float progress)
        {
            loadingBar.SetLoadingProgress(GetThresholdProgress(progress));
        }

        public void SetColoringProgress(float progress)
        {
            loadingBar.SetLoadingProgress(.5f + progress / 2);
        }

        private float GetThresholdProgress(float progress)
        {
            return progress / 1.92f;
        }
    }
}
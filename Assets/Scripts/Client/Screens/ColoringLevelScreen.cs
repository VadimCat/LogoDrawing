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

        public void SetCleaningProgress(float progress)
        {
            //TODO: MOVE FAKE PROGRESS LOGIC TO MODEL
            loadingBar.SetLoadingProgress(GetThresholdProgress(progress));
        }

        public void SetColoringProgress(float progress)
        {
            loadingBar.SetLoadingProgress(.5f + GetThresholdProgress(progress));
        }

        private float GetThresholdProgress(float progress)
        {
            return progress / 1.98f;
        }
    }
}
using Client.UI.Screens;
using Ji2Core.Core.ScreenNavigation;
using Ji2Core.UI.Screens;
using TMPro;
using UnityEngine;

namespace Client.Screens
{
    public class ColoringLevelScreen : BaseScreen
    {
        [SerializeField] private TextLoadingBar loadingBar;
        [SerializeField] private TMP_Text levelName;
        [SerializeField] private ParticleSystem cleanCompleteVFX;
        
        public void SetLevelName(string name)
        {
            levelName.text = name;
        }
        
        public void SetCleaningProgress(float progress)
        {
            loadingBar.SetLoadingProgress(GetThresholdProgress(progress));
        }
        
        public void UpdateCleaningProgress(float progress)
        {
            loadingBar.UpdateLoadingProgress(GetThresholdProgress(progress));
        }
        
        public void UpdateColoringProgress(float progress)
        {
            loadingBar.UpdateLoadingProgress(.5f + GetThresholdProgress(progress));
        }

        public void PlayCleaningCompleteVfx()
        {
            cleanCompleteVFX.Play();
        }
        
        private float GetThresholdProgress(float progress)
        {
            return progress / 2;
        }
    }
}
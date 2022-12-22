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
            loadingBar.SetLoadingProgressSmooth(GetThresholdProgress(progress));
        }

        public void SetColoringProgress(float progress)
        {
            loadingBar.SetLoadingProgressSmooth(.5f + GetThresholdProgress(progress));
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
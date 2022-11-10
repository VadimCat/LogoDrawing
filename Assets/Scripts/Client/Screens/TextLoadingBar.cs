using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Screens
{
    [Serializable]
    public class TextLoadingBar
    {
        [SerializeField] private Image loadingBar;
        [SerializeField] private TMP_Text progress;

        private const string ProgressTemplate = "{0}%";

        public void SetLoadingProgress(float normalProgress)
        {
            loadingBar.fillAmount = normalProgress;
            progress.text = string.Format(ProgressTemplate, (int)(normalProgress * 100));
        }
    }
}
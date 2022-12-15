using System;
using System.Globalization;
using DG.Tweening;
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

        public void AnimateProgress(float duration, TweenCallback OnAnimationEndedCallback)
        {
            loadingBar.DOFillAmount(1, duration)
                .OnComplete(OnAnimationEndedCallback)
                .OnUpdate(() =>
                {
                    Debug.Log(loadingBar.fillAmount);
                    progress.text = string.Format(ProgressTemplate, (loadingBar.fillAmount * 100).ToString("N0"));
                });
        }
    }
}
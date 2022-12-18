using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
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

        public async UniTask AnimateProgress(float duration)
        {
            var isAnimating = true;
            loadingBar.DOFillAmount(1, duration)
                .OnComplete(() => isAnimating = false)
                .OnUpdate(() =>
                {
                    progress.text = string.Format(ProgressTemplate, (loadingBar.fillAmount * 100).ToString("N0"));
                });
            await UniTask.WaitUntil(() => isAnimating == false);
        }
    }
}
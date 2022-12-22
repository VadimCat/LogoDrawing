using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Screens
{
    public class TextLoadingBar : MonoBehaviour
    {
        [SerializeField] private Image loadingBar;
        [SerializeField] private TMP_Text progress;
        private readonly List<Tween> fillAmountTweens = new();

        private const string ProgressTemplate = "{0}%";

        public void SetLoadingProgress(float normalProgress)
        {
            loadingBar.fillAmount = normalProgress;
            SetTextProgress(normalProgress);
        }

        public void SetLoadingProgressSmooth(float normalProgress)
        {
            SetTextProgress(normalProgress);
            fillAmountTweens.Add(loadingBar.DOFillAmount(normalProgress, .5f));
        }

        public async UniTask AnimateProgress(float duration)
        {
            await loadingBar.DOFillAmount(1, duration)
                .OnUpdate(() => { SetTextProgress(loadingBar.fillAmount); })
                .SetLink(gameObject)
                .AwaitForComplete();
        }

        private void SetTextProgress(float normalProgress)
        {
            progress.text = string.Format(ProgressTemplate, (normalProgress * 100).ToString("N0"));
        }

        private void OnDestroy()
        {
            foreach (var t in fillAmountTweens)
            {
                t.Kill();
            }
        }
    }
}
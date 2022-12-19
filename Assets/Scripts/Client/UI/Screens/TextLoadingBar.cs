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

        private const string ProgressTemplate = "{0}%";

        public void SetLoadingProgress(float normalProgress)
        {
            loadingBar.fillAmount = normalProgress;
            SetTextProgress(normalProgress);
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
    }
}
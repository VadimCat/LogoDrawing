using System;
using Client.Screens;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Ji2Core.UI.Screens;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI.Screens
{
    public class ColoringLevelScreen : BaseScreen
    {
        [SerializeField] private TextLoadingBar loadingBar;
        [SerializeField] private TMP_Text levelName;
        [SerializeField] private ParticleSystem cleanCompleteVFX;
        [SerializeField] private Button nextButton;
        [SerializeField] private TMP_Text nextButtonText;

        public event Action OnClickNext;
        private Sequence animationsSequence;

        private void Awake()
        {
            nextButton.onClick.AddListener(FireNext);
        }

        public void SetLevelName(string name)
        {
            levelName.text = name;
        }

        public void ShowNextButton()
        {
            const float duration = 0.3f;
            animationsSequence = DOTween.Sequence();
            animationsSequence.Join(nextButton.image.DOFade(1, duration).OnComplete(() => nextButton.interactable = true));
            animationsSequence.Join(nextButtonText.DOFade(1, duration));
            animationsSequence.Play();
        }

        public void HideNextButton()
        {
            var zeroAlpha = nextButton.image.color;
            zeroAlpha.a = 0;
            nextButton.image.color = zeroAlpha;
            nextButtonText.alpha = 0;
            nextButton.interactable = false;
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

        private async void FireNext()
        {
            await nextButton.transform.DOScale(0.9f, 0.1f).AwaitForComplete();
            Complete();
        }

        private void Complete()
        {
            OnClickNext?.Invoke();
        }
    }
}
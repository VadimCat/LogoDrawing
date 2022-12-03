using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Screens
{
    public class LevelCompletedScreen : BaseScreen
    {
        private const string levelNamePattern = "LEVEL {0}";
        [SerializeField] private Button nextButton;
        [SerializeField] private Image levelResult;
        [SerializeField] private TMP_Text levelName;
        [SerializeField] private Transform light0;

        public event Action OnClickNext;

        private void Awake()
        {
            AnimateBackLight();
            nextButton.onClick.AddListener(FireNext);
        }

        private void AnimateBackLight()
        {
            light0.DORotate(Vector3.back * 180, 8)
                .SetLoops(-1, LoopType.Incremental)
                .SetEase(Ease.Linear)
                .SetLink(gameObject);
            
            light0.DOScale(1.2f, 1)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.Linear)
                .SetLink(gameObject);

            levelResult.transform.DOScale(1.06f, 1)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.Linear)
                .SetLink(gameObject);

            levelResult.transform.DORotate(Vector3.forward * 2.2f, 1)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.Linear)
                .SetLink(gameObject);
        }

        public void SetLevelResult(Sprite levelResult, int levelNumber)
        {
            this.levelResult.sprite = levelResult;
            this.levelName.text = string.Format(levelNamePattern, levelNumber);
        }

        private void FireNext()
        {
            nextButton.transform.DOScale(0.9f, 0.1f)
            .OnComplete(Complete);
        }

        private void Complete()
        {
            OnClickNext?.Invoke();
        }

        private void OnDestroy()
        {
            OnClickNext = null;
        }
    }
}
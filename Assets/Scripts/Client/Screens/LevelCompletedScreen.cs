using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Screens
{
    public class LevelCompletedScreen : BaseScreen
    {
        [SerializeField] private Button nextButton;

        public event Action OnClickNext;

        private void Awake()
        {
            nextButton.onClick.AddListener(FireNext);
        }

        private void FireNext()
        {
            OnClickNext?.Invoke();
        }

        public override UniTask AnimateShow()
        {
            return UniTask.CompletedTask;
        }

        public override UniTask AnimateClose()
        {
            return UniTask.CompletedTask;
        }

        private void OnDestroy()
        {
            OnClickNext = null;
        }
    }
}
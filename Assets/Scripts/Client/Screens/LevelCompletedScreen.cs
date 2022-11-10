using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Screens
{
    public class LevelCompletedScreen : BaseScreen
    {
        [SerializeField] private Button nextButton;
        [SerializeField] private Image levelResult;
        [SerializeField] private TMP_Text levelName;
        
        public event Action OnClickNext;

        private void Awake()
        {
            nextButton.onClick.AddListener(FireNext);
        }

        public void SetLevelResult(Sprite levelResult, string levelName)
        {
            this.levelResult.sprite = levelResult;
            this.levelName.text = levelName;
        }
        
        private void FireNext()
        {
            OnClickNext?.Invoke();
        }
        
        private void OnDestroy()
        {
            OnClickNext = null;
        }
    }
}
using System;
using Data.ScriptableObjects;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UI
{
    public class ComplimentsWordsService : MonoBehaviour
    {
        [SerializeField] private TMP_Text complimentText;
        [SerializeField] private ComplimentsWordsAssets complimentsWordsAssets;

        [Header("Text Move Settings")] [SerializeField] [Range(0, 1)]
        private float distancePercentY = 0.5f;

        [SerializeField] [Range(0, 1)] private float distancePercentX = 0.5f;
        [SerializeField] private float moveDuration = 3;
        [SerializeField] private Ease moveEase = Ease.Linear;
        [SerializeField] private float fadeInDuration = 1;
        [SerializeField] private Ease fadeInEase = Ease.Linear;
        [SerializeField] private float fadeOutDuration = 1;
        [SerializeField] private Ease fadeOutEase = Ease.Linear;
        [SerializeField] private float rotateDuration = 3;
        [SerializeField] private float angle = 15;
        [SerializeField] private Ease rotationEase = default;

        public void ShowRandomFromScreenPosition(Vector2 startPosition)
        {
            complimentText.text = complimentsWordsAssets.GetRandomWord();
            complimentText.transform.position = startPosition;
            complimentText.color = complimentsWordsAssets.GetRandomColor();

            var targetPosition = GetTargetPosition(startPosition);
            var angleFactor = targetPosition.x > startPosition.x ? -1 : 1;
            StartTweensAnimations(targetPosition, angleFactor);
        }

        private void StartTweensAnimations(Vector2 targetPosition, int angleFactor)
        {
            complimentText.transform.rotation = Quaternion.identity;

            complimentText.alpha = 0;
            complimentText
                .DOFade(1, fadeOutDuration)
                .SetEase(fadeOutEase);

            complimentText.transform
                .DOMove(targetPosition, moveDuration)
                .SetEase(moveEase);

            complimentText.transform
                .DORotate(new Vector3(0, 0, angle * angleFactor), rotateDuration)
                .SetEase(rotationEase);

            complimentText
                .DOFade(0, fadeInDuration)
                .SetDelay(moveDuration - fadeInDuration)
                .SetEase(fadeInEase);
        }

        private Vector2 GetTargetPosition(Vector2 startPosition)
        {
            float x = Random.Range(0, Screen.width);
            float y = Screen.height;
            var distanceX = x - startPosition.x;
            var distanceY = y - startPosition.y;

            x = startPosition.x + distanceX * distancePercentX;
            y = startPosition.y + distanceY * distancePercentY;
            return new Vector2(x, y);
            ;
        }
    }
}
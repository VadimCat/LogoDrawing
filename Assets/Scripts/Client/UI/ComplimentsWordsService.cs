using System;
using Client.UI;
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
        [SerializeField] private ComplimentsWordsConfig complimentsWordsConfig;

        private Sequence animationsSequence;

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
            animationsSequence = DOTween.Sequence();
            animationsSequence.Join(complimentText
                .DOFade(1, complimentsWordsConfig.fadeOutDuration)
                .SetEase(complimentsWordsConfig.fadeOutEase));

            animationsSequence.Join(complimentText.transform
                .DOMove(targetPosition, complimentsWordsConfig.moveDuration)
                .SetEase(complimentsWordsConfig.moveEase));

            animationsSequence.Join(complimentText.transform
                .DORotate(new Vector3(0, 0, complimentsWordsConfig.angle * angleFactor),
                    complimentsWordsConfig.rotateDuration)
                .SetEase(complimentsWordsConfig.rotationEase));

            animationsSequence.Join(complimentText
                .DOFade(0, complimentsWordsConfig.fadeInDuration)
                .SetDelay(complimentsWordsConfig.moveDuration - complimentsWordsConfig.fadeInDuration)
                .SetEase(complimentsWordsConfig.fadeInEase));
            animationsSequence.Play();
        }

        private Vector2 GetTargetPosition(Vector2 startPosition)
        {
            float x = Random.Range(0, Screen.width);
            float y = Screen.height;
            var distanceX = x - startPosition.x;
            var distanceY = y - startPosition.y;

            x = startPosition.x + distanceX * complimentsWordsConfig.distancePercentX;
            y = startPosition.y + distanceY * complimentsWordsConfig.distancePercentY;
            return new Vector2(x, y);
        }
    }
}
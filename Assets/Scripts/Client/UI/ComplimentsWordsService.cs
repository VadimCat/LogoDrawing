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


        public void ShowRandomFromScreenPosition(Vector2 startPosition)
        {
            complimentText.text = complimentsWordsAssets.GetRandomWord();
            complimentText.transform.position = startPosition;
            complimentText.color = complimentsWordsAssets.GetRandomColor();
            complimentText.gameObject.SetActive(true);
            
            var targetPosition = GetTargetPosition(startPosition);

            complimentText.transform.DOMove(targetPosition, 1)
                .OnComplete(() => complimentText.gameObject.SetActive(false));
        }
        

        private Vector2 GetTargetPosition(Vector2 startPosition)
        {
            var x = Screen.width / 2;
            var y = Screen.height;
            var topScreenPosition = new Vector2(x, y);
            var directionVector = (topScreenPosition - startPosition).normalized;
            return startPosition + directionVector * Random.Range(250, 500);
        }
    }
}
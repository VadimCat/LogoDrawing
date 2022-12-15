using System;
using System.Collections.Generic;
using Data.ScriptableObjects;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.Client;
using Random = System.Random;

namespace UI
{
    public class ComplimentsWordsService : MonoBehaviour, IBootstrapable
    {
        private Random random;
        private List<string> congratsWords;
        private readonly Color[] colors = { Color.blue, Color.cyan, Color.red, Color.green, Color.yellow, };
        [SerializeField] private TMP_Text complimentText;
        [SerializeField] private CongratulationWords congratsWordsConfig;


        public void ShowRandomComplimentWordFromScreenPosition(Vector2 startPosition)
        {
            complimentText.text = GetRandomWord();
            complimentText.transform.position = startPosition;
            complimentText.color = GetRandomColor();
            complimentText.gameObject.SetActive(true);
            
            var targetPosition = GetTargetPosition(startPosition);

            complimentText.transform.DOMove(targetPosition, 1)
                .OnComplete(() => complimentText.gameObject.SetActive(false));
        }

        public void Bootstrap()
        {
            random = new Random();
            congratsWords = congratsWordsConfig.words;
        }

        private string GetRandomWord()
        {
            var index = random.Next(congratsWords.Count);
            return congratsWords[index];
        }

        private Color GetRandomColor()
        {
            var index = random.Next(colors.Length);
            return colors[index];
        }

        private Vector2 GetTargetPosition(Vector2 startPosition)
        {
            var x = Screen.width / 2;
            var y = Screen.height;
            var topScreenPosition = new Vector2(x, y);
            var directionVector = (topScreenPosition - startPosition).normalized;
            return startPosition + directionVector * random.Next(250, 500);
        }
    }
}
using System;
using System.Collections.Generic;
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
        const string WORDS_FILE_PATH = "CongratsWords";
        private List<string> congratsWords;
        [SerializeField] private TMP_Text complimentText;

        public void ShowRandomFromScreenPosition(Vector2 startPosition)
        {
            Debug.Log(startPosition);
            complimentText.text = GetRandomWord();
            complimentText.transform.position = startPosition;
            complimentText.gameObject.SetActive(true);

            var targetPosition = new Vector3(startPosition.x + startPosition.x * 0.2f,
                startPosition.y + startPosition.y * 0.2f, 10);

            complimentText.transform.DOMove(targetPosition, 1).OnComplete(() => complimentText.gameObject.SetActive(false));;
            //complimentText.DOFade(0, 1.5f).OnComplete(() => complimentText.gameObject.SetActive(false));
        }

        public void Bootstrap()
        {
            var words = LoadWords();
            congratsWords = JsonUtility.FromJson<CongratsWords>(words.ToString()).words;
        }

        private string GetRandomWord()
        {
            var random = new Random();
            var index = random.Next(congratsWords.Count);
            return congratsWords[index];
        }

        private TextAsset LoadWords()
        {
            return Resources.Load<TextAsset>(WORDS_FILE_PATH);
        }
    }

    [Serializable]
    public class CongratsWords
    {
        public List<string> words;
    }
}
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Data.ScriptableObjects
{
    [CreateAssetMenu]
    public class ComplimentsWordsAssets : ScriptableObject
    {
        private readonly Random random = new();

        [SerializeField] private List<string> words;
        [SerializeField] private List<Color> colors = new List<Color>(){};


        public string GetRandomWord()
        {
            var index = random.Next(words.Count);
            return words[index];
        }

        public Color GetRandomColor()
        {
            var index = random.Next(colors.Count);
            return colors[index];
        }
    }
}
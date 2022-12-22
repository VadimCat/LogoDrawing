using UnityEngine;

namespace Utils
{
    public class BackgroundsList
    {
        private const string BACKGROUND_INDEX_SAVE_KEY = "background_index";
        private readonly Sprite[] backgrounds;
        private int index;

        public BackgroundsList(Sprite[] backgrounds)
        {
            this.backgrounds = backgrounds;
            index = LoadLastIndex();
        }

        public Sprite GetNext()
        {
            index++;
            index = index == backgrounds.Length ? 0 : index;
            SaveLastIndex();
            return backgrounds[index];
        }

        private void SaveLastIndex() => PlayerPrefs.SetInt(BACKGROUND_INDEX_SAVE_KEY, index);
        private int LoadLastIndex() => PlayerPrefs.GetInt(BACKGROUND_INDEX_SAVE_KEY) - 1;
    }
}
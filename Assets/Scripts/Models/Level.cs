using System;
using Ji2Core.Models;
using UnityEngine;
using Utils;

namespace Models
{
    public class Level : ISavable
    {
        private const float COLORING_COMPLETE_THRESHOLD = .999f;
        //Used to show a bit faked overdone progress to avoid non-visible remaining pixels to paint.
        //Calculated as 1 / PRf : where PRf is progress to mark level as completed
        private const float LEVEL_PROGRESS_MULTIPLIER = 1.02f;
        private string id;
        public readonly int LevelPlayedTotal;

        public event Action OnColoringComplete;
        public event Action OnCleaningComplete;

        public string Id => id;

        public ReactiveProperty<ColoringStage> Stage => stage;

        private ReactiveProperty<ColoringStage> stage = new();

        public Level(string id, int levelPlayedTotal)
        {
            this.id = id;
            LevelPlayedTotal = levelPlayedTotal;
            Load();
        }

        public void UpdateColoringProgress(float progress, float oldValue)
        {
            progress = Mathf.Clamp01(progress * LEVEL_PROGRESS_MULTIPLIER);
            
            switch (stage.Value)
            {
                case ColoringStage.Cleaning:

                    if (progress >= COLORING_COMPLETE_THRESHOLD)
                    {
                        stage.Value = ColoringStage.Coloring;
                        Save();
                        OnCleaningComplete?.Invoke();
                    }

                    break;
                case ColoringStage.Coloring:
                    if (progress >= COLORING_COMPLETE_THRESHOLD)
                    {
                        OnColoringComplete?.Invoke();

                        ClearSave();
                    
                        OnColoringComplete = null;
                        OnCleaningComplete = null;
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Save()
        {
            PlayerPrefs.SetString(id, stage.Value.ToString());
        }

        public void Load()
        {
            if (PlayerPrefs.HasKey(id))
            {
                var stringStage = PlayerPrefs.GetString(id);

                if (Enum.TryParse<ColoringStage>(stringStage, out var value))
                {
                    stage = new ReactiveProperty<ColoringStage>(value);
                }
            }
        }

        public void ClearSave()
        {
            PlayerPrefs.DeleteKey(id);
        }
    }

    public enum ColoringStage
    {
        Cleaning,
        Coloring
    }
}
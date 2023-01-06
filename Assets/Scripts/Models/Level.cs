using System;
using System.Collections.Generic;
using Ji2Core.Core.Analytics;
using Ji2Core.Models;
using UnityEngine;
using Utils;

namespace Models
{
    public class Level : ISavable
    {
        private const string StartEvent = "level_start"; 
        private const string FinishEvent = "level_start"; 
        private const string LevelNumberKey = "level_number";
        private const string LevelNameKey = "level_name";
        private const string LevelCountKey = "level_count"; 
        private const string LevelLoopKey = "level_loop";
        private const string LevelRandomKey = "level_random";
        private const string LevelTypeKey = "level_type";
        private const string ResultKey = "result";
        private const string TimeKey = "time";


        //Used to show a bit faked overdone progress to avoid non-visible remaining pixels to paint.

        //Calculated as 1 / PRf : where PRf is progress to mark level as completed
        private const float LEVEL_PROGRESS_MULTIPLIER = 1.02f;
        private const float COLORING_COMPLETE_THRESHOLD = .999f;

        public readonly int LevelPlayedTotal;

        private readonly int levelLoop;
        private readonly Analytics analytics;

        public event Action OnColoringComplete;
        public event Action OnCleaningComplete;

        private readonly string id;

        public ReactiveProperty<ColoringStage> Stage => stage;

        private ReactiveProperty<ColoringStage> stage = new();
        private float playTime = 0;


        public Level(string id, int levelPlayedTotal, int levelLoop, Analytics analytics)
        {
            this.id = id;
            this.analytics = analytics;
            this.levelLoop = levelLoop;
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
                        OnCleaningComplete?.Invoke();
                        OnCleaningComplete = null;
                        Save();
                    }

                    break;
                case ColoringStage.Coloring:
                    if (progress >= COLORING_COMPLETE_THRESHOLD)
                    {
                        OnColoringComplete?.Invoke();
                        OnColoringComplete = null;
                        ClearSave();
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void AppendPlayTime(float time)
        {
            playTime += time;
            PlayerPrefs.SetFloat(TimeKey, playTime);
        }

        public void LogAnalyticsLevelStart()
        {
            var eventData = new Dictionary<string, object>
            {
                [LevelNumberKey] = LevelPlayedTotal,
                [LevelNameKey] = id,
                [LevelCountKey] = LevelPlayedTotal,
                [LevelLoopKey] = levelLoop
            };
            analytics.LogEventDirectlyTo<YandexMetricaLogger>(StartEvent, eventData);
            analytics.ForceSendDirectlyTo<YandexMetricaLogger>();
        }

        public void LogAnalyticsLevelFinish(LevelExitType levelExitType = LevelExitType.win)
        {
            var eventData = new Dictionary<string, object>
            {
                [LevelNumberKey] = LevelPlayedTotal,
                [LevelNameKey] = id,
                [LevelCountKey] = LevelPlayedTotal,
                [LevelLoopKey] = levelLoop,
                [ResultKey] = levelExitType.ToString(),
                [TimeKey] = playTime
            };

            analytics.LogEventDirectlyTo<YandexMetricaLogger>(FinishEvent, eventData);
            analytics.ForceSendDirectlyTo<YandexMetricaLogger>();
        }

        public void Save()
        {
            PlayerPrefs.SetString(id, stage.Value.ToString());
        }

        public void Load()
        {
            if (PlayerPrefs.HasKey(TimeKey))
            {
                playTime = PlayerPrefs.GetFloat(TimeKey);
                
                LogAnalyticsLevelFinish(LevelExitType.game_closed);
                PlayerPrefs.DeleteKey(TimeKey);
                
                Debug.LogError("LOG EXIT");
            }
            
            if (PlayerPrefs.HasKey(id))
            {
                var stringStage = PlayerPrefs.GetString(id);

                if (Enum.TryParse<ColoringStage>(stringStage, out var value))
                {
                    stage = new ReactiveProperty<ColoringStage>(value);
                }
            }
            else
            {
                playTime = 0;
            }
        }

        public void ClearSave()
        {
            PlayerPrefs.DeleteKey(TimeKey);
            PlayerPrefs.DeleteKey(id);
        }
        
    }

    public enum LevelExitType
    {
        // ReSharper disable once InconsistentNaming
        win,
        // ReSharper disable once InconsistentNaming
        game_closed
    }
    
    public enum ColoringStage
    {
        Cleaning,
        Coloring
    }
}
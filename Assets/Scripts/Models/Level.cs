using System;
using Utils;

public class Level
{
    private const float COLORING_COMPLETE_THRESHOLD = .8f;

    private string id;

    public event Action OnColoringComplete;
    public event Action OnCleaningComplete;

    public string Id => id;

    public ReactiveProperty<ColoringStage> Stage => stage;

    private readonly ReactiveProperty<ColoringStage> stage = new();

    public Level(string id)
    {
        this.id = id;
    }

    public void UpdateColoringProgress(float progress, float oldValue)
    {
        switch (stage.Value)
        {
            case ColoringStage.Clearing:
        
                if (progress >= COLORING_COMPLETE_THRESHOLD)
                {
                    stage.Value = ColoringStage.Coloring;
                    OnCleaningComplete?.Invoke();
                }
        
                break;
            case ColoringStage.Coloring:
        
                if (progress >= COLORING_COMPLETE_THRESHOLD)
                {
                    OnColoringComplete?.Invoke();

                    OnColoringComplete = null;
                    OnCleaningComplete = null;
                }
        
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}

public enum ColoringStage
{
    Clearing,
    Coloring
}
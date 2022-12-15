using UnityEngine;

public static class FloatExtensions
{
    public static float ToAudioLevel(this float value)
    {
        return Mathf.Log(value) * 20;
    }
}

using UnityEngine;

namespace Client.Audio
{
    [CreateAssetMenu]
    public class AudioConfig : ScriptableObject
    {
        [SerializeField] private float maxSfxLevel;
        [SerializeField] private float maxMusicLevel;
        
        public float MaxSfxLevel => maxSfxLevel;
        public float MaxMusicLevel => maxMusicLevel;
    }
}
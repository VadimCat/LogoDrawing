using UnityEngine;

namespace Client.Audio
{
    [CreateAssetMenu]
    public class AudioClipConfig : ScriptableObject
    {
        [SerializeField] private float playVolume = 1;
        [SerializeField] private AudioClip clip;
        [SerializeField] private AudioClipName clipName;

        public AudioClipName ClipName => clipName;
        public float PlayVolume => playVolume;
        public AudioClip Clip => clip;
    }
}
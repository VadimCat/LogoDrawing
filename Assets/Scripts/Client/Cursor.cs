using Client.Audio;
using Client.Audio.SfxPlayers;
using UnityEngine;

namespace Client
{
    public class Cursor : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particle;
        [SerializeField] private AudioClipName fxClip;

        private SfxPlaybackSource sfxPlayBackSource;

        public void SetDependencies(AudioService audioService)
        {
            sfxPlayBackSource = audioService.GetPlaybackSource(fxClip);
        }
        
        public void Play()
        {
            gameObject.SetActive(true);
            sfxPlayBackSource.PlaybackAsync(true);
            particle.Play();
        }

        public void Stop()
        {
            gameObject.SetActive(false);
            particle.Pause();
            sfxPlayBackSource.Pause();
        }
    }
}
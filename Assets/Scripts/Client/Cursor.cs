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
        private AudioService audioService;

        public void SetDependencies(AudioService audioService)
        {
            this.audioService = audioService;
            sfxPlayBackSource = audioService.GetPlaybackSource(fxClip);
        }
        
        public void Play()
        {
            gameObject.SetActive(true);
            sfxPlayBackSource.PlaybackAsync();
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
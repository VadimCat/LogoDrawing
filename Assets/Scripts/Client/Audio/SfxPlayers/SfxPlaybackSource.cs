using Client.Pools;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Client.Audio.SfxPlayers
{
    public class SfxPlaybackSource : MonoBehaviour, IPoolable
    {
        [SerializeField] private AudioSource source;
        private AudioClipConfig clipConfig;
        private UniTaskCompletionSource<bool> completionSource;
        
        public void SetDependencies(AudioClipConfig clipConfig)
        {
            this.clipConfig = clipConfig;
            source.clip = clipConfig.Clip;
            source.volume = clipConfig.PlayVolume;
        }
    
        public async UniTask PlaybackAsync()
        {
            source.Play();
            await UniTask.WaitWhile(IsPlaying);
        }

        public void Stop()
        {
            source.Stop();
        }
        
        private bool IsPlaying()
        {
            return source.isPlaying;
        }

        public void Spawn()
        {
            gameObject.SetActive(true);
        }

        public void DeSpawn()
        {
            gameObject.SetActive(false);

            source.Stop();
            source.clip = null;
        }
    }
}
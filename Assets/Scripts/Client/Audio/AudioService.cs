using UnityEngine;
using Utils;
using Utils.Client;

namespace Client.Audio
{
    public class AudioService : MonoBehaviour, IBootstrapable
    {
        private const string SFX_KEY = "SfxVolume";
        private const string MUSIC_KEY = "MusicVolume";

        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioConfig audioConfig;
        [SerializeField] private AudioClipsConfig clipsConfig;

        private AudioSettings audioSettings;

        public ReactiveProperty<float> sfxVolume;
        public ReactiveProperty<float> musicVolume;

        public void Bootstrap()
        {
            audioSettings = new();
            //TODO: May be inject from outside
            clipsConfig.Bootstrap();

            sfxVolume = new ReactiveProperty<float>((audioSettings.SfxLevel * audioConfig.MaxSfxLevel).ToAudioLevel());
            musicVolume = new ReactiveProperty<float>((audioSettings.MusicLevel * audioConfig.MaxMusicLevel).ToAudioLevel());

            sfxSource.outputAudioMixerGroup.audioMixer.SetFloat(SFX_KEY, sfxVolume.Value);
            musicSource.outputAudioMixerGroup.audioMixer.SetFloat(MUSIC_KEY, musicVolume.Value);

            SetSfxLevel(audioSettings.SfxLevel);
            SetMusicLevel(audioSettings.MusicLevel);
        }

        public void SetSfxLevel(float level)
        {
            var groupVolume = (audioConfig.MaxSfxLevel * level).ToAudioLevel();
            musicSource.outputAudioMixerGroup.audioMixer.SetFloat(SFX_KEY, groupVolume);

            audioSettings.SfxLevel = level;
            sfxVolume.Value = level;
            
            audioSettings.Save();
        }

        public void SetMusicLevel(float level)
        {
            var groupVolume = (audioConfig.MaxMusicLevel * level).ToAudioLevel();
            sfxSource.outputAudioMixerGroup.audioMixer.SetFloat(MUSIC_KEY, groupVolume);
            
            audioSettings.MusicLevel = level;
            musicVolume.Value = level;
            
            audioSettings.Save();
        }

        public void PlayMusic(AudioClipName clipName)
        {
            var clipConfig = clipsConfig.GetClip(clipName);
            musicSource.clip = clipConfig.Clip;
            musicSource.volume = clipConfig.PlayVolume;
            musicSource.Play();
            musicSource.clip.LoadAudioData();
        }
        
        public void PlaySfx(AudioClipName clipName)
        {
            var clipConfig = clipsConfig.GetClip(clipName);
            musicSource.PlayOneShot(clipConfig.Clip, clipConfig.PlayVolume);
            musicSource.clip.LoadAudioData();
        }
    }

    public enum AudioClipName
    {
        DefaultBackgroundMusic,
        ButtonFX,
        WinFX,
        CleaningFX,
        ColoringFX
    }
}
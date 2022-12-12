﻿using System.Collections.Generic;
using UnityEngine;
using Utils.Client;

namespace Client.Audio
{
    [CreateAssetMenu]
    public class AudioClipsConfig : ScriptableObject, IBootstrapable
    {
        [SerializeField] private AudioClipConfig[] clips;
        
        private Dictionary<AudioClipName, AudioClipConfig> clipsDict = new();
        
        public void Bootstrap()
        {
            foreach (var clip in clips)
            {
                clipsDict[clip.ClipName] = clip;
            }
        }
        
        public AudioClipConfig GetClip(AudioClipName clipName)
        {
            return clipsDict[clipName];
        }
    }
}
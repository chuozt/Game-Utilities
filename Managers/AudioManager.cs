// =================================
// Copyright (c) 2025 Pham Huu Gia Hai (aka chuozt)
// All rights reserved. 
// You may not copy, distribute, or publish these scripts 
// without explicit permission from the copyright holder.
// =================================
    
using System;
using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace Chuozt.Template
{
    public class AudioManager : Singleton<AudioManager>
    {
        [Header("===== Audio Sources =====")]
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private float pitchShift = 0.1f;

        [Space(10)]
#if ODIN_INSPECTOR
        [Title("===== SFX Clips Grouped =====")]
        [ListDrawerSettings(DraggableItems = true)]
#endif
        [SerializeField] private List<SFXGroup> sfxGroups = new List<SFXGroup>();

        private Dictionary<AudioKey, List<AudioClip>> sfxDict;

        [Serializable]
        public class SFXGroup
        {
#if ODIN_INSPECTOR
            [HorizontalGroup("Group")]
            [LabelWidth(100), HideLabel]
#endif
            public string groupName;

#if ODIN_INSPECTOR
            [HorizontalGroup("Group")]
#endif
            public List<AudioClip> clips = new List<AudioClip>();
        }

#if ODIN_INSPECTOR
        [Button("Generate Audio Keys", 30)]
        void GenerateAudioKeys() => AudioKeyGenerator.GenerateAudioKeyEnum(this);
#endif

        public List<SFXGroup> SFXGroups => sfxGroups;

        private void OnEnable() => InitializeSFXDictionary();

        private void InitializeSFXDictionary()
        {
            sfxDict = new Dictionary<AudioKey, List<AudioClip>>();

            foreach (var group in sfxGroups)
            {
                if (string.IsNullOrWhiteSpace(group.groupName))
                    continue;

                var key = ParseAudioKey(group.groupName);
                if (!sfxDict.ContainsKey(key))
                    sfxDict.Add(key, group.clips);
                else
                    Debug.LogWarning($"AudioManager: Duplicate SFX group name '{group.groupName}' detected!");
            }
        }

        public void PlaySFX(AudioKey key, bool isPitchShift = true)
        {
            if (!GameDataManager.GetSFXValue())
                return;

            if (sfxDict == null)
                return;

            sfxSource.pitch = 1;

            if (sfxDict.TryGetValue(key, out List<AudioClip> clips))
            {
                if (clips.Count == 0) return;

                var randomClip = clips[UnityEngine.Random.Range(0, clips.Count)];

                if (isPitchShift)
                    sfxSource.pitch = UnityEngine.Random.Range(1 - pitchShift, 1 + pitchShift);

                sfxSource.PlayOneShot(randomClip);
            }
        }

        public void PlaySFX(AudioKey key, float pitchOverride)
        {
            if (!GameDataManager.GetSFXValue())
                return;

            if (sfxDict == null)
                return;

            if (sfxDict.TryGetValue(key, out List<AudioClip> clips))
            {
                if (clips.Count == 0) return;

                var randomClip = clips[UnityEngine.Random.Range(0, clips.Count)];

                sfxSource.pitch = pitchOverride;
                sfxSource.PlayOneShot(randomClip);
            }
        }

        private AudioKey ParseAudioKey(string groupName)
        {
            if (Enum.TryParse(groupName, true, out AudioKey key))
                return key;

            return default; // fallback to None
        }
    }
}
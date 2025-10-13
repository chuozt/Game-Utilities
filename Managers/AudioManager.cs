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
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private float pitchShift = 0.1f;

#if ODIN_INSPECTOR
        [Space(10)]
        [Title("===== SFX Clips Grouped =====")]
        [ListDrawerSettings(DraggableItems = true)]
        [SerializeField] private List<SFXGroup> sfxGroups = new List<SFXGroup>();

        private Dictionary<AudioKey, List<AudioClip>> sfxDict;

        [Serializable]
        public class SFXGroup
        {
            [HorizontalGroup("Group"), LabelWidth(100), HideLabel] public string groupName;
            [HorizontalGroup("Group")] public List<AudioClip> clips = new List<AudioClip>();
        }

        [Space(20)]
        [Title("===== Music Clips Grouped =====")]
        [ListDrawerSettings(DraggableItems = true)]
        [SerializeField] private List<MusicGroup> musicGroups = new List<MusicGroup>();

        private Dictionary<MusicKey, List<AudioClip>> musicDict;

        [Serializable]
        public class MusicGroup
        {
            [HorizontalGroup("Group"), LabelWidth(100), HideLabel] public string groupName;
            [HorizontalGroup("Group")] public List<AudioClip> clips = new List<AudioClip>();
        }

        [Button("Generate SFX Keys", 30)]
        private void GenerateSFXKeys() => AudioKeyGenerator.GenerateAudioKeyEnum(this);

        [Button("Generate Music Keys", 30)]
        private void GenerateMusicKeys() => MusicKeyEditor.GenerateMusicKeyEnum(this);

        public List<SFXGroup> SFXGroups => sfxGroups;
        public List<MusicGroup> MusicGroups => musicGroups;

#else
        [SerializeField] private List<SFXGroup> sfxGroups = new List<SFXGroup>();
        private Dictionary<AudioKey, List<AudioClip>> sfxDict;

        [Serializable]
        public class SFXGroup
        {
            [SerializeField] public string groupName;
            [SerializeField] public List<AudioClip> clips = new List<AudioClip>();
        }

        [SerializeField] private List<MusicGroup> musicGroups = new List<MusicGroup>();
        private Dictionary<MusicKey, List<AudioClip>> musicDict;

        [Serializable]
        public class MusicGroup
        {
            [SerializeField] public string groupName;
            [SerializeField] public List<AudioClip> clips = new List<AudioClip>();
        }

        public List<SFXGroup> SFXGroups => sfxGroups;
        public List<MusicGroup> MusicGroups => musicGroups;
#endif

        private void OnEnable()
        {
            InitializeSFXDictionary();
            InitializeMusicDictionary();
        }

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

        private void InitializeMusicDictionary()
        {
            musicDict = new Dictionary<MusicKey, List<AudioClip>>();

            foreach (var group in musicGroups)
            {
                if (string.IsNullOrWhiteSpace(group.groupName))
                    continue;

                var key = ParseMusicKey(group.groupName);
                if (!musicDict.ContainsKey(key))
                    musicDict.Add(key, group.clips);
                else
                    Debug.LogWarning($"AudioManager: Duplicate Music group name '{group.groupName}' detected!");
            }
        }

        public void PlaySFX(AudioKey key, bool isPitchShift = true)
        {
            if (!GameDataManager.GetSFXValue() || sfxDict == null)
                return;

            if (!sfxDict.TryGetValue(key, out List<AudioClip> clips) || clips.Count == 0)
                return;

            var randomClip = clips[UnityEngine.Random.Range(0, clips.Count)];
            sfxSource.pitch = isPitchShift
                ? UnityEngine.Random.Range(1 - pitchShift, 1 + pitchShift)
                : 1f;

            sfxSource.PlayOneShot(randomClip);
        }

        public void PlaySFX(AudioKey key, float pitchOverride)
        {
            if (!GameDataManager.GetSFXValue() || sfxDict == null)
                return;

            if (!sfxDict.TryGetValue(key, out List<AudioClip> clips) || clips.Count == 0)
                return;

            var randomClip = clips[UnityEngine.Random.Range(0, clips.Count)];
            sfxSource.pitch = pitchOverride;
            sfxSource.PlayOneShot(randomClip);
        }

        public void PlayMusic(MusicKey key, bool random = true, bool loop = true)
        {
            if (!GameDataManager.GetMusicValue() || musicDict == null)
                return;

            if (!musicDict.TryGetValue(key, out List<AudioClip> clips) || clips.Count == 0)
                return;

            var randomClip = random? clips[UnityEngine.Random.Range(0, clips.Count)] : clips[0]; // Take the 1st if not random
            musicSource.clip = randomClip;
            musicSource.loop = loop;
            musicSource.Play();
        }

        public void StopMusic() => musicSource.Stop();

        private AudioKey ParseAudioKey(string groupName)
        {
            if (Enum.TryParse(groupName, true, out AudioKey key))
                return key;

            return default;
        }

        private MusicKey ParseMusicKey(string groupName)
        {
            if (Enum.TryParse(groupName, true, out MusicKey key))
                return key;

            return default;
        }
    }
}

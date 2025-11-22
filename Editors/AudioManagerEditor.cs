// =================================
// Copyright (c) 2025 Pham Huu Gia Hai (aka chuozt)
// All rights reserved. 
// You may not copy, distribute, or publish these scripts 
// without explicit permission from the copyright holder.
// =================================

using UnityEngine;
using UnityEditor;

namespace Chuozt.Template
{
#if !ODIN_INSPECTOR && UNITY_EDITOR
    [CustomEditor(typeof(AudioManager))]
    public class AudioManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Generate Audio Keys", GUILayout.Height(30)))
                AudioKeyGenerator.GenerateAudioKeyEnum((AudioManager)target);
        }
    }
#endif
}
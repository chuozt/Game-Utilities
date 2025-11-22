// =================================
// Copyright (c) 2025 Pham Huu Gia Hai (aka chuozt)
// All rights reserved. 
// You may not copy, distribute, or publish these scripts 
// without explicit permission from the copyright holder.
// =================================

using System.IO;
using System.Text;
using Chuozt.Template.ProUtils;
using UnityEditor;
using UnityEngine;


namespace Chuozt.Template
{
    public static class MusicKeyEditor
    {
#if UNITY_EDITOR
        private const string PrefKey = "Chuozt_MusicKey_Folder";
        private const string DefaultPath = "Assets/Scripts/Managers";

        [MenuItem("ProUtils/Set MusicKey.cs Output Folder")]
        private static void SetOutputFolder()
        {
            string path = EditorUtility.OpenFolderPanel("Select MusicKey Output Folder", Application.dataPath, "");
            if (!string.IsNullOrEmpty(path))
            {
                // Convert absolute path to relative
                if (path.StartsWith(Application.dataPath))
                    path = "Assets" + path.Substring(Application.dataPath.Length);

                EditorPrefs.SetString(PrefKey, path);
                Debug.Log($"📂 MusicKey output folder set to: {path}");
            }
        }
#endif

        public static void GenerateMusicKeyEnum(AudioManager manager)
        {
            // Get output folder
            string folderPath = EditorPrefs.GetString(PrefKey, DefaultPath);

            // If folder is invalid or missing, fallback to default
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                Debug.LogWarning($"⚠️ Saved folder {folderPath} is missing. Falling back to Assets/Scripts");
                folderPath = "Assets/Scripts";
                if (!AssetDatabase.IsValidFolder(folderPath))
                    AssetDatabase.CreateFolder("Assets", "Scripts");
            }

            string filePath = Path.Combine(folderPath, "MusicKey.cs");

            // Build enum file
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("public enum MusicKey");
            sb.AppendLine("{");

            foreach (var group in manager.MusicGroups)
            {
                if (string.IsNullOrWhiteSpace(group.groupName))
                    continue;

                string safeName = StringHelper.ToPascalCase(group.groupName);
                sb.AppendLine($"    {safeName},");
            }

            sb.AppendLine("}");

            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
            AssetDatabase.Refresh();

            Debug.Log($"✅ MusicKey.cs regenerated at: {filePath}");
        }
    }
}
// =================================
// Copyright (c) 2025 Pham Huu Gia Hai (aka chuozt)
// All rights reserved. 
// You may not copy, distribute, or publish these scripts 
// without explicit permission from the copyright holder.
// =================================

using System.IO;
using System.Text;
using Chuozt.Template;
using Chuozt.Template.ProUtils;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public static class AudioKeyGenerator
{
    private const string PrefKey = "Chuozt_AudioKey_Folder";
    private const string DefaultPath = "Assets/Scripts/Managers";

    [MenuItem("ProUtils/Set AudioKey.cs Output Folder")]
    private static void SetOutputFolder()
    {
        string path = EditorUtility.OpenFolderPanel("Select AudioKey Output Folder", Application.dataPath, "");
        if (!string.IsNullOrEmpty(path))
        {
            // Convert absolute path to relative project path
            if (path.StartsWith(Application.dataPath))
                path = "Assets" + path.Substring(Application.dataPath.Length);

            EditorPrefs.SetString(PrefKey, path);
            Debug.Log($"📂 AudioKey output folder set to: {path}");
        }
    }

    public static void GenerateAudioKeyEnum(AudioManager manager)
    {
        // If user has set a folder, use it; else default
        string folderPath = EditorPrefs.GetString(PrefKey, DefaultPath);

        // If the folder doesn’t exist anymore, recreate it
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            Debug.LogWarning($"⚠️ Saved folder {folderPath} is missing. Falling back to Assets/Scripts");
            folderPath = "Assets/Scripts";
            if (!AssetDatabase.IsValidFolder(folderPath))
                AssetDatabase.CreateFolder("Assets", "Scripts");
        }

        string filePath = Path.Combine(folderPath, "AudioKey.cs");

        // Build enum
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("public enum AudioKey");
        sb.AppendLine("{");

        foreach (var group in manager.SFXGroups)
        {
            if (string.IsNullOrWhiteSpace(group.groupName))
                continue;

            string safeName = StringHelper.ToPascalCase(group.groupName);
            sb.AppendLine($"    {safeName},");
        }

        sb.AppendLine("}");

        File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
        AssetDatabase.Refresh();

        Debug.Log($"✅ AudioKey.cs regenerated at: {filePath}");
    }
}
#endif
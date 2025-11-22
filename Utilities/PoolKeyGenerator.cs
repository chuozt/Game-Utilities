// =================================
// Copyright (c) 2025 Pham Huu Gia Hai (aka chuozt)
// All rights reserved. 
// You may not copy, distribute, or publish these scripts 
// without explicit permission from the copyright holder.
// =================================

using System.Collections.Generic;
using System.IO;
using System.Text;
using Chuozt.Template;
using Chuozt.Template.ProUtils;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public static class PoolKeyGenerator
{
    private const string PrefKey = "Chuozt_PoolKey_Folder";
    private const string DefaultPath = "Assets/Scripts/Managers";

    [MenuItem("ProUtils/Set PoolKey.cs Output Folder")]
    private static void SetOutputFolder()
    {
        string path = EditorUtility.OpenFolderPanel("Select PoolKey Output Folder", Application.dataPath, "");
        if (!string.IsNullOrEmpty(path))
        {
            // Convert absolute path to relative project path
            if (path.StartsWith(Application.dataPath))
                path = "Assets" + path.Substring(Application.dataPath.Length);

            EditorPrefs.SetString(PrefKey, path);
            Debug.Log($"📂 PoolKey output folder set to: {path}");
        }
    }

    public static void GeneratePoolKeyEnum(PoolManager manager)
    {
        string filePath;

        // 1. Search for existing PoolKey.cs
        string[] guids = AssetDatabase.FindAssets("PoolKey t:Script");
        if (guids.Length > 0)
            filePath = AssetDatabase.GUIDToAssetPath(guids[0]);
        else
        {
            // Use saved folder if available, else default
            string folderPath = EditorPrefs.GetString(PrefKey, DefaultPath);

            // If the folder doesn’t exist anymore, recreate it
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                Debug.LogWarning($"⚠️ Saved folder {folderPath} is missing. Falling back to Assets/Scripts");
                folderPath = "Assets/Scripts";

                if (!AssetDatabase.IsValidFolder(folderPath))
                {
                    AssetDatabase.CreateFolder("Assets", "Scripts");
                    folderPath = "Assets/Scripts";
                }
            }

            filePath = Path.Combine(folderPath, "PoolKey.cs");
        }

        // 2. Build enum content
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("public enum PoolKey");
        sb.AppendLine("{");

        var poolListField = manager.GetType()
            .GetField("poolList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        if (poolListField != null)
        {
            var poolList = poolListField.GetValue(manager) as List<Pool>;
            if (poolList != null)
            {
                foreach (var pool in poolList)
                {
                    string keyName = pool.poolKey.ToString();
                    if (string.IsNullOrWhiteSpace(keyName))
                        continue;

                    string safeName = StringHelper.ToPascalCase(keyName);
                    sb.AppendLine($"    {safeName},");
                }
            }
        }

        sb.AppendLine("}");

        // 3. Write file
        File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
        AssetDatabase.Refresh();

        Debug.Log($"✅ PoolKey.cs regenerated at: {filePath}");
    }
}
#endif
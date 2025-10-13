// =================================
// Copyright (c) 2025 Pham Huu Gia Hai (aka chuozt)
// All rights reserved. 
// You may not copy, distribute, or publish these scripts 
// without explicit permission from the copyright holder.
// =================================
    
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Chuozt.Template
{
    public class SceneBrowserWindow : EditorWindow
    {
        private Vector2 scrollPos;

        // ✅ Leave this empty to show all scenes
        private static readonly string[] allowedSceneNames = new string[]
        {
            //"MainMenu",
            //"Gameplay"
        };

        private static readonly string folder = "Assets/Scenes";

        private List<string> scenePaths = new();

        [MenuItem("ProUtils/Select Scenes")]
        public static void ShowWindow()
        {
            GetWindow<SceneBrowserWindow>("Filtered Scenes");
        }

        private void OnEnable()
        {
            RefreshSceneLists();
        }

        private void OnGUI()
        {
            GUILayout.Label("Filtered Scene Browser", EditorStyles.boldLabel);

            if (GUILayout.Button("Refresh Scene List"))
                RefreshSceneLists();

            GUILayout.Space(10);
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            DrawSceneList("===== Scenes =====", scenePaths);
            EditorGUILayout.EndScrollView();
        }

        private void DrawSceneList(string header, List<string> paths)
        {
            GUILayout.Label(header, EditorStyles.boldLabel);

            foreach (string path in paths)
            {
                string sceneName = Path.GetFileNameWithoutExtension(path);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(sceneName);

                if (GUILayout.Button("Open", GUILayout.Width(60)))
                {
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                        EditorSceneManager.OpenScene(path);
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        private void RefreshSceneLists()
        {
            scenePaths = GetFilteredScenes(folder);
        }

        private List<string> GetFilteredScenes(string folder)
        {
            List<string> results = new();
            string[] guids = AssetDatabase.FindAssets("t:Scene", new[] { folder });

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                string sceneName = Path.GetFileNameWithoutExtension(path);

                if (allowedSceneNames.Length == 0 || allowedSceneNames.Any(filter => sceneName.Contains(filter)))
                    results.Add(path);
            }

            results.Sort();
            return results;
        }
    }
}
#endif

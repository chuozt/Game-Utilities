// =================================
// Copyright (c) 2025 Pham Huu Gia Hai (aka chuozt)
// All rights reserved. 
// You may not copy, distribute, or publish these scripts 
// without explicit permission from the copyright holder.
// =================================
    
using System;
using UnityEngine;

namespace Chuozt.Template
{
    public static class GameDataManager
    {
        private const string KEY_SFX = "SFX";
        private const string KEY_MUSIC = "MUSIC";

        public static event Action onNotEnoughCoin;
        public static event Action onCoinChanged;

        public static void ToggleSFX() => PlayerPrefs.SetInt(KEY_SFX, GetSFXValue() ? 0 : 1);

        public static bool GetSFXValue() => PlayerPrefs.GetInt(KEY_SFX, 1) == 1;

        public static void ToggleMusic() => PlayerPrefs.SetInt(KEY_MUSIC, GetMusicValue() ? 0 : 1);

        public static bool GetMusicValue() => PlayerPrefs.GetInt(KEY_MUSIC, 1) == 1;
    }

    public static class ScenesName
    {
        public const string START_LOADING = "1_StartLoading";
        public const string MENU = "2_Menu";
        public const string GAME = "3_Game";
    }
}
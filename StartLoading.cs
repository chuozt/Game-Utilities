// =================================
// Copyright (c) 2025 Pham Huu Gia Hai (aka chuozt)
// All rights reserved. 
// You may not copy, distribute, or publish these scripts 
// without explicit permission from the copyright holder.
// =================================

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Chuozt.Template
{
    public class StartLoading : MonoBehaviour
    {
        void Start()
        {
            StartCoroutine(LoadToMainHome());
        }

        private IEnumerator LoadToMainHome(float delayOnFinished = 1)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(ScenesName.MENU);
            asyncLoad.allowSceneActivation = false;

            while (!asyncLoad.isDone)
            {
                float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);

                if (asyncLoad.progress >= 0.9f)
                {
                    yield return new WaitForSeconds(0.5f);
                    asyncLoad.allowSceneActivation = true;
                }

                yield return null;
            }

            yield return new WaitForSeconds(delayOnFinished);
        }
    }
}
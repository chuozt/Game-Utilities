// =================================
// Copyright (c) 2025 Pham Huu Gia Hai (aka chuozt)
// All rights reserved. 
// You may not copy, distribute, or publish these scripts 
// without explicit permission from the copyright holder.
// =================================
    
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

namespace Chuozt.Template
{
    public class BoxLoading : BaseBox
    {
        private static BoxLoading instance;

        public static void LoadScene(string sceneName, float delayOnFinished = 1)
        {
            instance = Setup();
            instance.sceneToLoad = sceneName;
            instance.StartCoroutine(instance.LoadSceneAsync(delayOnFinished));
        }

        public static BoxLoading Setup()
        {
            if (instance == null)
            {
                instance = Instantiate(Resources.Load<BoxLoading>(PathsPrefab.BOX_LOADING));
                DontDestroyOnLoad(instance.gameObject);
            }

            instance.gameObject.SetActive(true);
            return instance;
        }

        [SerializeField] private Image bg;
        [SerializeField] private Image thumbnail;
        [SerializeField] private TMP_Text textLoading;

        private Coroutine loadingTextCoroutine;
        private string sceneToLoad;
        private Vector2 initialPosText;
        private Vector2 offsetTextLoading = new Vector2(0, -100);

        void Awake()
        {
            initialPosText = textLoading.rectTransform.anchoredPosition;
        }

        public override void OnOpen()
        {
            bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, 0);
            thumbnail.DOFade(0, 0);
            textLoading.DOFade(0, 0);
            textLoading.rectTransform.anchoredPosition = initialPosText + offsetTextLoading;

            base.OnOpen();

            if (loadingTextCoroutine != null)
                StopCoroutine(loadingTextCoroutine);

            loadingTextCoroutine = StartCoroutine(AnimateLoadingText());

            //Animate
            Sequence sequence = DOTween.Sequence().SetUpdate(true);

            sequence.Append(bg.DOFade(1, 0.25f));
            sequence.Join(thumbnail.DOFade(1, 0.5f));
            sequence.Join(textLoading.DOFade(1, 0.5f));
            sequence.Join(textLoading.rectTransform.DOAnchorPos(initialPosText, 0.5f).SetEase(Ease.OutBack));
        }

        public override void OnClose()
        {
            base.OnClose();

            if (loadingTextCoroutine != null)
            {
                StopCoroutine(loadingTextCoroutine);
                loadingTextCoroutine = null;
            }

            //Animate
            Sequence sequence = DOTween.Sequence().SetUpdate(true);

            sequence.Append(textLoading.DOFade(0, 0.5f));
            sequence.Join(textLoading.rectTransform.DOAnchorPos(initialPosText + offsetTextLoading, 0.5f).SetEase(Ease.InBack));
            sequence.Join(bg.DOFade(0, 0.5f));
            sequence.Join(thumbnail.DOFade(0, 0.5f));

            sequence.OnComplete(() => gameObject.SetActive(false));
        }

        private IEnumerator LoadSceneAsync(float delayOnFinished = 1)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);
            asyncLoad.allowSceneActivation = false;
            OnOpen();

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
            OnClose(); // Hide after load finishes
        }

        private IEnumerator AnimateLoadingText()
        {
            int dotCount = 0;
            while (true)
            {
                dotCount = (dotCount + 1) % 4; // cycle from 0 to 3
                textLoading.text = "Loading" + new string('.', dotCount);
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : PersistenSingleten<SceneLoader>
{
    [SerializeField] UnityEngine.UI.Image transitionImage;
    [SerializeField] float fadeTime = 3.5f;

    Color color;

    const string GAMEPLAY = "GamePlay";
    const string MAIN_MENU = "MainMenu";
    const string SCORING = "Scoring";

    IEnumerator LoadingCoroutine(string sceneName)
    {
        //Load new scene in background
        var loadingOperation = SceneManager.LoadSceneAsync(sceneName);
        //Set this scene inactive
        loadingOperation.allowSceneActivation = false;

        //Fade out
        transitionImage.gameObject.SetActive(true);
        
        while(color.a < 1f)
        {
            color.a = Mathf.Clamp01(color.a + Time.unscaledDeltaTime / fadeTime);
            transitionImage.color = color;

            yield return null;
        }

        //挂起等待直到场景加载好了
        yield return new WaitUntil(() => loadingOperation.progress >= 0.9f);

        //Active the new scene
        loadingOperation.allowSceneActivation = true;

        //Fade in
        while(color.a > 0f)
        {
            color.a = Mathf.Clamp01(color.a - Time.unscaledDeltaTime / fadeTime);
            transitionImage.color = color;

            yield return null;
        }

        transitionImage.gameObject.SetActive(false);
    }

    public void LoadGamePlayScene()
    {
        StopAllCoroutines();    //防止频繁的加载场景从而导致携程的冲突，在启用新携程前需要先禁用携程
        StartCoroutine(LoadingCoroutine(GAMEPLAY));
    }

    public void LoadMainMenuScene()
    {
        StopAllCoroutines();    //防止频繁的加载场景从而导致携程的冲突，在启用新携程前需要先禁用携程
        StartCoroutine(LoadingCoroutine(MAIN_MENU));
    }

    public void LoadScoringScene()
    {
        StopAllCoroutines();
        StartCoroutine(LoadingCoroutine(SCORING));
    }
}

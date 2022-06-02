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

        //����ȴ�ֱ���������غ���
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
        StopAllCoroutines();    //��ֹƵ���ļ��س����Ӷ�����Я�̵ĳ�ͻ����������Я��ǰ��Ҫ�Ƚ���Я��
        StartCoroutine(LoadingCoroutine(GAMEPLAY));
    }

    public void LoadMainMenuScene()
    {
        StopAllCoroutines();    //��ֹƵ���ļ��س����Ӷ�����Я�̵ĳ�ͻ����������Я��ǰ��Ҫ�Ƚ���Я��
        StartCoroutine(LoadingCoroutine(MAIN_MENU));
    }

    public void LoadScoringScene()
    {
        StopAllCoroutines();
        StartCoroutine(LoadingCoroutine(SCORING));
    }
}

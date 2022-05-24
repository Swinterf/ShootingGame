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

    IEnumerator LoadCoroutine(string sceneName)
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
        StartCoroutine(LoadCoroutine(GAMEPLAY));
    }
}

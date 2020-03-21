using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class LoadSceneManager : MonoSingleton<LoadSceneManager> 
{
    public float LoadCompleteDelay = 0.2f;

    public void LoadScene(string levelName,Action callback=null)
    {
        SceneManager.LoadScene(levelName);
        callback?.Invoke();
    }

    public void LoadSceneAsync(string levelName,Action onComplete)
    {
        StartCoroutine(LoadSceneAsyncIE(levelName, onComplete));
    }

    IEnumerator LoadSceneAsyncIE(string levelName, Action onComplete)
    {
        bool complete = false;
        UICallBack callback = (objs) =>
        {
            complete = true;
        };
        UILoadingWindow loadingWindow= UIManager.Instance.OpenWindow<UILoadingWindow>(true, callback);
        while (!complete)
        {
            yield return null;
        }
        var asyncOperation = SceneManager.LoadSceneAsync(levelName);
        asyncOperation.allowSceneActivation = false;
        while (asyncOperation.progress<0.9f)
        {
            loadingWindow.SetProgress(asyncOperation.progress/9*10);
            yield return null;
        }
        loadingWindow.SetProgress(1);
        while (loadingWindow.Progress<1)
        {
            yield return null;
        }
        yield return new WaitForSeconds(LoadCompleteDelay);
        UIManager.Instance.CloseWindow(loadingWindow);
        asyncOperation.allowSceneActivation = true;
        onComplete?.Invoke();
    }
}

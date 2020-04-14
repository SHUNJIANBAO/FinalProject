using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class LoadSceneManager : MonoSingleton<LoadSceneManager>
{
    public float LoadCompleteDelay = 0.2f;

    public void LoadScene(int levelId, Action callback = null)
    {
        PlayerData.Instance.CurPlayerInfo.CurLevelId = levelId;
        string levelName = "Level_" + levelId;
        UICallBack uiCallback = (args) =>
        {
            SceneManager.LoadScene(levelName);
            callback?.Invoke();
            CloseTrashCamera();
            UIManager.Instance.CloseWindow<UILoadingWindow>();
        };
        UIManager.Instance.OpenWindow<UILoadingWindow>(true, uiCallback);
    }

    public void LoadSceneAsync(int levelId, Action onComplete = null)
    {
        PlayerData.Instance.CurPlayerInfo.CurLevelId = levelId;
        string levelName = "Level_" + levelId;
        UIManager.Instance.CloseAllWindow();
        StartCoroutine(LoadSceneAsyncIE(levelName, onComplete));
    }

    IEnumerator LoadSceneAsyncIE(string levelName, Action onComplete)
    {
        bool complete = false;
        UICallBack callback = (objs) =>
        {
            complete = true;
        };
        UILoadingWindow loadingWindow = UIManager.Instance.OpenWindow<UILoadingWindow>(true, callback);
        while (!complete)
        {
            yield return null;
        }
        var asyncOperation = SceneManager.LoadSceneAsync(levelName);
        asyncOperation.completed += (op) => { CloseTrashCamera(); onComplete?.Invoke(); };
        asyncOperation.allowSceneActivation = false;
        while (asyncOperation.progress < 0.9f)
        {
            loadingWindow.SetProgress(asyncOperation.progress / 9 * 10);
            yield return null;
        }
        loadingWindow.SetProgress(1);
        while (loadingWindow.Progress < 1)
        {
            yield return null;
        }
        yield return new WaitForSeconds(LoadCompleteDelay);
        UIManager.Instance.CloseWindow(loadingWindow);
        asyncOperation.allowSceneActivation = true;
    }

    void CloseTrashCamera()
    {
        var cams = GameObject.FindObjectsOfType<Camera>();
        for (int i = 0; i < cams.Length; i++)
        {
            if (cams[i].tag == "MainCamera" && !CameraManager.Instance.IsMainCamera(cams[i]))
                cams[i].gameObject.SetActive(false);
        }
    }

}

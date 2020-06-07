using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class LoadSceneManager : MonoSingleton<LoadSceneManager>
{
    public float LoadCompleteDelay = 0.5f;

    public void LoadScene(int levelId, Action callback = null)
    {
        PlayerData.Instance.CurPlayerInfo.CurLevelId = levelId;
        string levelName = "Level_" + levelId;
        var levelCfg = LevelConfig.GetData(levelId);
        UICallBack uiCallback = (args) =>
        {
            SceneManager.LoadScene(levelName);
            if (!string.IsNullOrEmpty(levelCfg.BGM))
            {
                AudioManager.Instance.PlayBGM(levelCfg.BGM);
            }
            callback?.Invoke();
            UIManager.Instance.CloseWindow<UILoadingWindow>();
        };
        UIManager.Instance.OpenWindow<UILoadingWindow>(true, uiCallback);
    }

    public void LoadSceneAsync(int levelId, Action onComplete = null)
    {
        PlayerData.Instance.CurPlayerInfo.CurLevelId = levelId;
        UIManager.Instance.CloseAllWindow();
        StartCoroutine(LoadSceneAsyncIE(levelId, onComplete));
    }

    IEnumerator LoadSceneAsyncIE(int levelId, Action onComplete)
    {
        string levelName= "Level_" + levelId;
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
        asyncOperation.completed += (op) => { onComplete?.Invoke(); };
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
        GameManager.CurLevelId = levelId;
    }


}

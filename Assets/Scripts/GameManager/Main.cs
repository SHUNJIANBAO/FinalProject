using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Main : MonoSingleton<Main>
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        ApplicationEnter();
    }

    /// <summary>
    /// 程序启动
    /// </summary>
    public void ApplicationEnter()
    {
        AudioManager.Instance.LoadAudios(PathManager.AudioPath);
        UIManager.Instance.OpenWindow<UILogoWindow>();
    }

    public void OnGameEnter()
    {

    }
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.J))
    //    {
    //        //UIManager.Instance.OpenWindow<TestWindow>();

    //    }
    //    if (Input.GetKeyDown(KeyCode.K))
    //    {
    //       // UIManager.Instance.CloseWindow<TestWindow>();

    //    }
    //}
}

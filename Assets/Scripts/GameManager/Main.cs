using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Main : MonoSingleton<Main>
{
    public Camera MainCamera;

    private void Awake()
    {
        MainCamera = GetComponentInChildren<Camera>();
        DontDestroyOnLoad(gameObject);
        ApplicationEnter();
    }

    /// <summary>
    /// 程序启动
    /// </summary>
    public void ApplicationEnter()
    {
        LoadAssets(); //加载资源
        LoadConfigs();  //加载配置

        UIManager.Instance.OpenWindow<UILogoWindow>();
    }

    void LoadAssets()
    {
        AudioManager.Instance.LoadAudios(PathManager.AudioPath);
    }

    void LoadConfigs()
    {
       
        ItemConfig.LoadCsvCfg();
        AnimConfig.LoadCsvCfg();
        RoleConfig.LoadCsvCfg();
        BuffConfig.LoadCsvCfg();
        BulletConfig.LoadCsvCfg();

        PlayerData.Load();
        GameData.Load();
    }

    public void OnGameEnter()
    {
        
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.J))
        //{
        //    LoadSceneManager.Instance.LoadSceneAsync("Level_1", null);
        //}
        if (Input.GetKeyDown(KeyCode.K))
        {
            PlayerData.Instance.SavePlayerInfo(PlayerData.Instance.CurPlayerInfo);
        }
    }
}

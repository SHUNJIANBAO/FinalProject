using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

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
        //todo delete
        //ClearData();

        LoadAssets(); //加载资源
        LoadConfigs();  //加载配置
        LoadData();

        UIManager.Instance.OpenWindow<UILogoWindow>();
    }

    void LoadAssets()
    {
        AudioManager.Instance.LoadAudios(PathManager.AudioPath);
    }

    public void LoadConfigs()
    {
       
        ItemConfig.LoadCsvCfg();
        AnimConfig.LoadCsvCfg();
        RoleConfig.LoadCsvCfg();
        BuffConfig.LoadCsvCfg();
        SkillConfig.LoadCsvCfg();
        LevelConfig.LoadCsvCfg();
        BulletConfig.LoadCsvCfg();
        ColliderConfig.LoadCsvCfg();

    }

    void LoadData()
    {
        PlayerData.Load();
        GameData.Load();
    }

    void ClearData()
    {
        PlayerData.Clear();
        GameData.Clear();
    }

    public void OnGameEnter()
    {
        
    }

    //private void Start()
    //{
    //    var obj= Resources.Load<GameObject>("Prefabs/Roles/XiaoWen/XiaoWen");
    //    var go = GameObject.Instantiate(obj);
    //    var cha= go.GetComponent<Character>();
    //    MonoBehaviourManager.Add(cha);
    //}

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

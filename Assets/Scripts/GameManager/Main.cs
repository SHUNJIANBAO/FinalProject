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
        LoadData(); //加载数据

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
        PlayerConfig.LoadCsvCfg();
        MonsterConfig.LoadCsvCfg();
        BossConfig.LoadCsvCfg();
        BuffConfig.LoadCsvCfg();
        SkillConfig.LoadCsvCfg();
        LevelConfig.LoadCsvCfg();
        BulletConfig.LoadCsvCfg();
        BarrageConfig.LoadCsvCfg();
        ColliderConfig.LoadCsvCfg();

    }

    void LoadData()
    {
        PlayerData.Load();
        GameData.Load();
        LevelData.Load();
    }

    void ClearData()
    {
        PlayerData.Clear();
        GameData.Clear();
        LevelData.Clear();
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
    EmitterManager _curEmitter;
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.J))
        //{
        //    if (_curEmitter != null)
        //        PoolManager.DestroyGameObject(_curEmitter.gameObject, PoolType.EmitterManager);
        //    //LoadSceneManager.Instance.LoadSceneAsync("Level_1", null);
        //    GameObject go = ResourceManager.Load<GameObject>(PathManager.GetBulletPath("GameObject"));
        //    _curEmitter = ShootManager.Instance.Shoot(transform, transform.position, Vector3.right, go, E_BarrageType.Parallel, 4, 3, 1, 1, 1, 0);
        //}
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    if (_curEmitter != null)
        //        PoolManager.DestroyGameObject(_curEmitter.gameObject, PoolType.EmitterManager);
        //    GameObject go = ResourceManager.Load<GameObject>(PathManager.GetBulletPath("GameObject"));
        //    _curEmitter = ShootManager.Instance.Shoot(transform, transform.position, Vector3.right, go, E_BarrageType.Sector, 6, 3, 0, 10, 4, 1);
        //}
        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    if (_curEmitter != null)
        //        PoolManager.DestroyGameObject(_curEmitter.gameObject, PoolType.EmitterManager);
        //    GameObject go = ResourceManager.Load<GameObject>(PathManager.GetBulletPath("GameObject"));
        //    _curEmitter = ShootManager.Instance.Shoot(transform, transform.position, Vector3.right, go, E_BarrageType.Sector, 36, 3, 0.1f, 10, 1, 4);
        //    //PlayerData.Instance.SavePlayerInfo(PlayerData.Instance.CurPlayerInfo);
        //}

    }
}

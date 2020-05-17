using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoint : MonoBehaviour 
{
    public int EnemyId;
    public enum EnemyType
    {
        怪物,
        Boss,
    }

    public EnemyType Type;

    private void Start()
    {
        string path = "";
        switch (Type)
        {
            case EnemyType.怪物:
                MonsterConfig mCfg = MonsterConfig.GetData(EnemyId);
                path = PathManager.RolePrefabsPath + mCfg.Name + "/" + mCfg.Name;
                break;
            case EnemyType.Boss:
                BossConfig bCfg = BossConfig.GetData(EnemyId);
                path = PathManager.RolePrefabsPath + bCfg.Name + "/" + bCfg.Name;
                break;
        }
        var obj = ResourceManager.Load<GameObject>(path);
        var go = GameObject.Instantiate(obj);
        go.transform.SetParent(transform, false);
        var character = go.GetComponent<Character>();
        MonoBehaviourManager.Add(character);
    }
}

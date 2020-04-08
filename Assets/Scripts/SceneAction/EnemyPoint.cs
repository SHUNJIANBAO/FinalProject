using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoint : MonoBehaviour 
{
    public int EnemyId;
    private void Start()
    {
        RoleConfig rCfg = RoleConfig.GetData(EnemyId);
        string path = PathManager.RolePrefabsPath+ rCfg.Name+"/"+ rCfg.Name;
        var obj = ResourceManager.Load<GameObject>(path);
        var go = GameObject.Instantiate(obj);
        go.transform.SetParent(transform, false);
        var character = go.GetComponent<Character>();
        MonoBehaviourManager.Add(character);
    }
}

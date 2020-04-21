using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPoint : MonoBehaviour
{
    [HideInInspector]
    public int PlayerId = 1010;
    private void Start()
    {
        var player = MonoBehaviourManager.GetById(PlayerId);
        if (player == null)
        {
            RoleConfig rCfg = RoleConfig.GetData(PlayerId);
            string path = PathManager.RolePrefabsPath + rCfg.Name + "/" + rCfg.Name;
            var obj = ResourceManager.Load<GameObject>(path);
            var go = GameObject.Instantiate(obj);
            go.transform.position = transform.position;
            DontDestroyOnLoad(go);
            var character = go.GetComponent<Character>();
            MonoBehaviourManager.Add(character);
            CameraManager.SetFollowTarget(character.transform);
        }
        else
        {
            player.transform.position = transform.position;
            CameraManager.SetFollowTarget(player.transform);
        }
    }

    private void OnDestroy()
    {
        CameraManager.SetFollowTarget(null);
    }
}

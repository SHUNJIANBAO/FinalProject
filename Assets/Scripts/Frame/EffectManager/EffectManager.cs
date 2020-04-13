using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    public GameObject Play(string effName, Vector3 offest, Transform parent, float lifeTime = 0)
    {
        string effPath = PathManager.GetEffectPath(effName);
        var eff = PoolManager.InstantiateGameObject(effPath, PoolType.Effect);
        eff.transform.SetParent(parent);
        eff.transform.localPosition = offest;
        eff.transform.localScale = parent.localScale;
        if (parent.localScale.x > 0)
        {
            eff.transform.position = parent.transform.position + offest;
        }
        else
        {
            eff.transform.position = parent.transform.position + new Vector3(-offest.x, offest.y, offest.z);
        }
        var ctrl = eff.GetComponent<EffectCtrl>();
        EffectConfig cfg = new EffectConfig();
        cfg.LifeTime = lifeTime;
        ctrl.Init(cfg);
        return eff;
    }

    public GameObject Play(string effName, Vector3 pos,bool isFaceRight, float lifeTime = 0)
    {
        string effPath = PathManager.GetEffectPath(effName);
        var eff = PoolManager.InstantiateGameObject(effPath, PoolType.Effect);
        eff.transform.position = pos;
        if (isFaceRight)
        {
            eff.transform.localScale = Vector3.one;
        }
        else
        {
            eff.transform.localScale = new Vector3(-1, 1, 1);
        }
        var ctrl = eff.GetComponent<EffectCtrl>();
        EffectConfig cfg = new EffectConfig();
        cfg.LifeTime = lifeTime;
        ctrl.Init(cfg);
        return eff;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    public GameObject Play(string effName, Vector3 offest, Transform parent, float lifeTime = 0)
    {
        if (string.IsNullOrEmpty(effName))
        {
            return null;
        }
        string effPath = PathManager.GetEffectPath(effName);
        var eff = PoolManager.InstantiateGameObject(effPath, PoolType.Effect);
        if (parent!=null)
        {
            eff.transform.SetParent(parent);
            eff.transform.localScale = parent.localScale;
            if (parent.localScale.x > 0)
            {
                eff.transform.localPosition =  offest;
            }
            else
            {
                eff.transform.localPosition = new Vector3(-offest.x, offest.y, offest.z);
            }
        }
        else
        {
            eff.transform.position = offest;
        }
        var ctrl = eff.GetComponent<EffectCtrl>()??eff.AddComponent<EffectCtrl>();
        EffectConfig cfg = new EffectConfig();
        cfg.LifeTime = lifeTime;
        MonoBehaviourManager.Add(ctrl, cfg);
        return eff;
    }

    public GameObject Play(string effName, Vector3 pos, bool isFaceRight, float lifeTime = 0)
    {
        if (string.IsNullOrEmpty(effName))
        {
            return null;
        }
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
        MonoBehaviourManager.Add(ctrl, cfg);
        return eff;
    }
}

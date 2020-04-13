using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectCtrl : MonoEntity 
{
    Animator _anim;
    AnimatorStateInfo _curState;

    EffectConfig _cfg;
    float _lifeTime;
    float _timeCount;

    protected override void OnStart()
    {
        base.OnStart();
        _anim = GetComponent<Animator>();
    }

    protected override void OnInit(params object[] objs)
    {
        base.OnInit(objs);
        _timeCount = 0;
        _cfg = (EffectConfig)objs[0];
        _anim.Play("Play", 0, 0);
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (_cfg.LifeTime!=0)
        {
            _timeCount += GameManager.DeltaTime;
            if (_timeCount>_cfg.LifeTime)
            {
                Destroy();
            }
        }
        else
        {
            _curState = _anim.GetCurrentAnimatorStateInfo(0);
            if (_curState.normalizedTime >= 1f)
            {
                Destroy();
            }
        }
    }

    void Destroy()
    {
        PoolManager.DestroyGameObject(gameObject, PoolType.Effect);
    }
}

public struct EffectConfig
{
    public float LifeTime;
}

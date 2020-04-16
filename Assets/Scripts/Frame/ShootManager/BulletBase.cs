using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletBase : MonoEntity
{
    protected float _damage;
    protected override void OnInit(params object[] objs)
    {
        base.OnInit(objs);
        _damage = float.Parse(objs[0].ToString());
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        Move();
    }
    protected abstract void Move();
}

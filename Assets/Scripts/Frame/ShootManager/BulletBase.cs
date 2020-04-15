using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletBase : MonoEntity 
{
    protected override void OnUpdate()
    {
        base.OnUpdate();
        Move();
    }
    protected abstract void Move();
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMoveForward : BulletBase
{
    public override void Move()
    {
        transform.Translate(Vector3.right * m_BulletCfg.MoveSpeed * GameManager.DeltaTime * m_Animator.speed);
    }

}

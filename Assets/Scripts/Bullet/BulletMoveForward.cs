using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMoveForward : BulletBase
{
    protected override void Move()
    {
        transform.Translate(Vector3.right * 10 * Time.deltaTime);
    }
}

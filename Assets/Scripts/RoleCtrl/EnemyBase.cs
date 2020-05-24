using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : Character
{
    float _collisionTimeCount;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == GameConfig.Player.gameObject)
        {
            if (!GameConfig.Player.IsGround)
            {
                GameConfig.Player.HurtOnCollision(gameObject);
            }
            _collisionTimeCount = 0;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject == GameConfig.Player.gameObject)
        {
            //if (GameConfig.Player.IsGround)
            //{
                _collisionTimeCount += Time.deltaTime;
                if (_collisionTimeCount > GameConfig.Instance.CollisionDamageWaitTime)
                {
                    GameConfig.Player.HurtOnCollision(gameObject);
                    _collisionTimeCount = 0;
                }
            //}
        }
    }

}

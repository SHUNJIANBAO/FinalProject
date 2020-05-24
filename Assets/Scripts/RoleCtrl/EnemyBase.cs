using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : Character
{
    float _collisionTimeCount;
    bool _judge;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == GameManager.Player.gameObject)
        {
            //if (!GameManager.Player.IsGround)
            //{
            //    GameManager.Player.HurtOnCollision(gameObject);
            //}
            _collisionTimeCount = 0;
            _judge = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject == GameManager.Player.gameObject)
        {
            //if (GameManager.Player.IsGround)
            //{
                _collisionTimeCount += Time.deltaTime;
                if (_collisionTimeCount > GameConfig.Instance.CollisionDamageWaitTime)
                {
                    GameManager.Player.HurtOnCollision(gameObject);
                    _collisionTimeCount = 0;
                }
            //}
            if (GameManager.Player.CurStatus==E_CharacterFsmStatus.Jump&& !_judge&&!GameManager.Player.IsGround)
            {
                _judge = true;
                GameManager.Player.HurtOnCollision(gameObject);
            }
        }
    }

}

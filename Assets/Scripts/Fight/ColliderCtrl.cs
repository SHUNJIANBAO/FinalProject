using UnityEngine;

public class ColliderCtrl : MonoBehaviour
{

    ColliderConfig _coliderCfg;
    BoxCollider2D _box;
    Character _owner;

    int _damage;
    int _hitForce;
    string _hitEffect;
    E_HitEffectPosType _hitPosType;

    float _timeCount;
    float _intervalTimeCount;

    protected void Awake()
    {
        _box = GetComponent<BoxCollider2D>();
    }

    public void Init(int colliderId, Character owner, int damage, int hitForce, string hitEffect, E_HitEffectPosType hitPosType)
    {
        _timeCount = 0;
        _damage = damage;
        _hitForce = hitForce;
        _hitEffect = hitEffect;
        _hitPosType = hitPosType;
        _owner = owner;
        _coliderCfg = ColliderConfig.GetData(colliderId);
        _intervalTimeCount = _coliderCfg.DamageInterval;
        _box.size = _coliderCfg.Size;
        switch (_coliderCfg.LifeType)
        {
            case E_ColliderFollowType.None:
                transform.position = _owner.transform.position + (Vector3)_coliderCfg.Offest;
                break;
            case E_ColliderFollowType.Follow:
                transform.SetParent(_owner.transform, false);
                if (_owner.IsFaceRight)
                {
                    transform.localPosition = _coliderCfg.Offest;
                }
                else
                {
                    transform.localPosition = new Vector2(-_coliderCfg.Offest.x, _coliderCfg.Offest.y);
                }
                break;
        }
    }

    private void Update()
    {
        _timeCount += GameManager.DeltaTime;
        if (_timeCount > _coliderCfg.LifeTime)
        {
            Destroy();
        }
        if (_coliderCfg.DamageType == E_DamageType.Repeat && _coliderCfg.DamageInterval != 0)
        {
            _intervalTimeCount += GameManager.DeltaTime;
            if (_intervalTimeCount > _coliderCfg.DamageInterval)
            {
                _intervalTimeCount = 0;
                var hit = Physics2D.BoxCast(transform.position, _box.size, 0, Vector2.right);
                if (hit)
                {
                    var target = hit.collider.GetComponent<Character>();
                    if (target != null && target != _owner)
                    {
                        Damage(target);
                    }
                }
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_coliderCfg.DamageType == E_DamageType.Repeat) return;

        var target = collision.GetComponent<Character>();
        if (target != null && target != _owner)
        {
            Damage(target);
            if (_coliderCfg.DamageType == E_DamageType.Once)
            {
                Destroy();
            }
        }
    }

    void Damage(Character target)
    {
        var movement = target.GetComponent<CharacterMovement>();
        if (_coliderCfg.IsAttacker)
        {
            movement.Hurt(gameObject, _damage, _hitForce);
        }
        else
        {
            movement.Hurt(_owner.gameObject, _damage, _hitForce);
        }

        switch (_hitPosType)
        {
            case E_HitEffectPosType.HitPoint:
                Vector2 dir = target.transform.position - _owner.transform.position;
                var hit = Physics2D.Raycast(_owner.transform.position, dir, 5);
                if (hit.transform.GetComponent<Character>() == target)
                {
                    EffectManager.Instance.Play(_hitEffect, hit.point, _owner.IsFaceRight);
                }
                break;
            case E_HitEffectPosType.CharacterCenter:
                EffectManager.Instance.Play(_hitEffect, Vector3.zero, target);
                break;
        }
    }

    void Destroy()
    {
        PoolManager.DestroyGameObject(gameObject, PoolType.Collider);
    }


#if UNITY_EDITOR
    public int ColliderId = 0;
    protected void OnDrawGizmosUpdate()
    {
        if (!Application.isPlaying && ColliderId != 0)
        {
            var cfg = ColliderConfig.GetData(ColliderId);
            UnityEditor.Handles.color = Color.red;
            UnityEditor.Handles.DrawWireCube(transform.position + (Vector3)cfg.Offest, cfg.Size);
        }
    }
#endif
}

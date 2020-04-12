using UnityEngine;

public class ColliderCtrl : MonoBehaviour
{

    ColliderConfig _coliderCfg;
    BoxCollider2D _box;
    Character _owner;

    int _damage;
    int _hitForce;

    float _timeCount;
    float _intervalTimeCount;

    protected void Start()
    {
        _box = GetComponent<BoxCollider2D>();
    }

    public void Init(int colliderId, Character owner, int damage, int hitForce)
    {
        _timeCount = 0;
        _damage = damage;
        _hitForce = hitForce;
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

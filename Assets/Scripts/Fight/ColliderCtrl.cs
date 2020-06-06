using UnityEngine;

public class ColliderCtrl : MonoBehaviour
{

    ColliderConfig _coliderCfg;
    BoxCollider2D _box;
    Character _owner;

    int _damage;
    int _hitForce;
    string _hitEffect;
    float _hitEffectLife;
    string _hitAudio;
    E_HitEffectPosType _hitPosType;

    float _timeCount;
    float _intervalTimeCount;

    protected void Awake()
    {
        _box = GetComponent<BoxCollider2D>();
    }

    public void Init(int colliderId, Character owner, int damage, int hitForce, SkillConfig skill)
    {
        _timeCount = 0;
        _damage = damage;
        _hitForce = hitForce;
        _hitEffect = skill.HitEffect;
        _hitPosType = skill.HitEffectPosType;
        _hitAudio = skill.HitAudio;
        _hitEffectLife = skill.HitEffectLife;
        _owner = owner;
        _coliderCfg = ColliderConfig.GetData(colliderId);
        _intervalTimeCount = _coliderCfg.DamageInterval;
        _box.size = _coliderCfg.Size;
        switch (_coliderCfg.LifeType)
        {
            case E_ColliderFollowType.None:
                transform.SetParent(null);
                if (_owner.IsFaceRight)
                {
                    transform.position = _owner.transform.position + (Vector3)_coliderCfg.Offest;
                }
                else
                {
                    transform.position = _owner.transform.position + new Vector3(-_coliderCfg.Offest.x, _coliderCfg.Offest.y, 0);
                }
                break;
            case E_ColliderFollowType.Follow:
                transform.SetParent(_owner.transform, false);
                //if (_owner.IsFaceRight)
                //{
                transform.localPosition = _coliderCfg.Offest;
                //}
                //else
                //{
                //    transform.localPosition = new Vector2(-_coliderCfg.Offest.x, _coliderCfg.Offest.y);
                //}
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
        if (_coliderCfg.IsAttacker)
        {
            target.Hurt(gameObject, _damage, _hitForce);
        }
        else
        {
            target.Hurt(_owner.gameObject, _damage, _hitForce);
        }

        if (!string.IsNullOrEmpty(_hitAudio))
        {
            AudioManager.Instance.PlayAudio(_hitAudio);//, target.gameObject);
        }
        if (!string.IsNullOrEmpty(_hitEffect))
        {
            switch (_hitPosType)
            {
                case E_HitEffectPosType.HitPoint:
                    Vector2 dir = target.transform.position - _owner.transform.position;
                    var hitArry = Physics2D.RaycastAll(_owner.transform.position, dir, 5);
                    foreach (var hit in hitArry)
                    {
                        if (hit.transform.GetComponent<Character>() == target)
                        {
                            EffectManager.Instance.Play(_hitEffect, hit.point, _owner.IsFaceRight, _hitEffectLife);
                        }
                    }
                    break;
                case E_HitEffectPosType.CharacterCenter:
                    EffectManager.Instance.Play(_hitEffect, target.transform.position, true, _hitEffectLife);
                    break;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (RuntimeTest.Instance.DrawCollider && gameObject.activeSelf)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(transform.position, _box.size);
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

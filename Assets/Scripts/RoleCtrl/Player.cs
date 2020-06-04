using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    PlayerConfig _cfg;
    bool _jumpOnHead;
    protected SpriteRenderer m_SpriteRender;

    protected override void OnInit(params object[] objs)
    {
        base.OnInit(objs);
        m_SpriteRender = GetComponent<SpriteRenderer>();
    }

    protected override void RegistAttribute()
    {
        base.RegistAttribute();

        _cfg = PlayerConfig.GetData(Id);

        gameObject.name = _cfg.Name;
        var _hp = new GameRangeAttribute(E_Attribute.Hp.ToString(), 0, _cfg.Hp);
        m_Hp = AddRangeAttribute(_hp);
        var _mp = new GameRangeAttribute(E_Attribute.Mp.ToString(), 0, _cfg.Mp, 0.1f);
        m_Mp = AddRangeAttribute(_mp);
        var _attack = new GameAttribute(E_Attribute.Atk.ToString(), _cfg.Attack);
        m_Attack = AddAttribute(_attack);
        var _moveSpeed = new GameAttribute(E_Attribute.MoveSpeed.ToString(), _cfg.MoveSpeed);
        m_MoveSpeed = AddAttribute(_moveSpeed);

        m_CaculateDelta = TimerManager.Instance.AddListener(0, 0.02f, m_MonoAttribute.CaculateRangeDelta, null, true);


        UIBattleWindow.OnPlayerHpChange(m_Hp.Current, m_Hp.GetMinTotalValue(), m_Hp.GetMaxTotalValue());
        UIBattleWindow.OnPlayerMpChange(m_Mp.Current, m_Mp.GetMinTotalValue(), m_Mp.GetMaxTotalValue());
        GetRangeAttribute(E_Attribute.Hp.ToString()).OnValueChanged += (delta) => { UIBattleWindow.OnPlayerHpChange(m_Hp.Current, m_Hp.GetMinTotalValue(), m_Hp.GetMaxTotalValue()); };
        GetRangeAttribute(E_Attribute.Mp.ToString()).OnValueChanged += (delta) => { UIBattleWindow.OnPlayerMpChange(m_Mp.Current, m_Mp.GetMinTotalValue(), m_Mp.GetMaxTotalValue()); };

        GameManager.Player = this;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (IsGround)
        {
            _jumpOnHead = false;
        }
    }

    public override void Hurt(GameObject atkOwner, int damage, int hitForce)
    {
        if (IsInvincible)
        {
            if (CurStatus == E_CharacterFsmStatus.Attack)
            {
                if (!GameManager.IsTimeStop)
                {
                    GameManager.StopOtherTime(null, true);
                    Util.RunLater(this, () => { GameManager.StopOtherTime(null, false); }, GameConfig.Instance.HitTimeStop);
                }
                CameraManager.Ripple(transform.position);
                CameraManager.ShakeCamera();
            }
            return;
        }
        //if (CurStatus == E_CharacterFsmStatus.FallDown) return;
        base.Hurt(atkOwner, damage, hitForce);
        LookToTarget(atkOwner.transform.position);


        BattleHandles.HitFeel(this, GameConfig.Instance.HitTimeStop);


        //if (hitForce > 0)
        //{
        //    ChangeStatus(E_CharacterFsmStatus.HitFly, true);
        //}
        //else
        //{
        ChangeStatus(E_CharacterFsmStatus.Hurt, true);
        IsInvincible = true;
        Glint(9, 0.15f, () => IsInvincible = false);
        //}

    }

    public void HurtOnCollision(GameObject atkOwner)
    {
        if (CheckJumpOnHead() && !_jumpOnHead)
        {
            _jumpOnHead = true;
            Movement.Jump(15, true, SceneConfigManager.Instance.PlayJumpDownEffect);
        }
        else
        {
            if (!_jumpOnHead && !IsGround)
            {
                return;
            }
            _jumpOnHead = false;
            Hurt(atkOwner, Mathf.CeilToInt(m_Hp.GetMaxTotalValue() * 0.2f), 5);
        }
    }

    /// <summary>
    /// 碰撞时判断是否跳跃
    /// </summary>
    bool CheckJumpOnHead()
    {
        var hit = Physics2D.BoxCast((Vector2)transform.position + BottomOffest, new Vector2(BoxCollider.size.x + 0.1f, 0.2f), 0, Vector2.right, 0, GameConfig.Instance.EnemyMask);
        if (hit)
        {
            var target = hit.transform.GetComponent<Character>();
            if (CanJumpHead(target))
            {
                return true;
            }
        }
        return false;
    }

    bool CanJumpHead(Character target)
    {
        bool onHead = transform.position.y - target.transform.position.y > target.BoxCollider.size.y - 0.2f;
        bool isJumpDown = CurStateInfo.IsName(E_AnimatorIndex.JumpingDown.ToString());
        return onHead && isJumpDown;
    }

    protected override void OnDrawGizmosUpdate()
    {
        if (Application.isPlaying)
        {
            Gizmos.DrawCube(transform.position + (Vector3)BottomOffest, new Vector3(BoxCollider.size.x, 0.2f, 1));
        }
    }

    IEnumerator Glint(int count, float interval, System.Action callback = null)
    {
        int isClear = count % 2 == 0 ? 1 : 0;
        while (count > 0)
        {
            count--;
            if (count % 2 == isClear)
            {
                m_SpriteRender.color = Color.clear;
            }
            else
            {
                m_SpriteRender.color = Color.white;
            }
            yield return new WaitForSeconds(interval);
        }
        m_SpriteRender.color = Color.white;
        callback?.Invoke();
    }

}

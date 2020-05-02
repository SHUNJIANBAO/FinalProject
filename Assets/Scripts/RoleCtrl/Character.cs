using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(BehaviourTree))]
[RequireComponent(typeof(CharacterMovement))]
public class Character : MonoEntity
{
    [Header("角色类型")]
    public RoleType RoleType;
    [Header("角色当前状态")]
    public E_CharacterFsmStatus CurStatus;
    [Header("是否在地面上")]
    public bool IsGround;
    [Header("是否无敌")]
    public bool IsInvincible;
    [Header("攻击目标")]
    public Character AttackTarget;
    [Header("移动目标点")]
    public Vector3 MoveTarget;
    //是否面向右
    public bool IsFaceRight => transform.localScale.x > 0;

    public SkillConfig CurSkill;
    [HideInInspector]
    public Rigidbody2D Rigibody;
    protected Animator m_Animator;
    public AnimatorStateInfo CurStateInfo
    {
        get
        {
            return m_Animator.GetCurrentAnimatorStateInfo(0);
        }
    }
    protected BoxCollider2D m_BoxCollider;
    public BoxCollider2D BoxCollider
    {
        get
        {
            return m_BoxCollider;
        }
    }

    FsmManager fsm = new FsmManager();
    protected int m_CurSkillId;
    protected Vector2 m_TopOffest;
    protected Vector2 m_BottomOffest;
    public Vector2 BottomOffest
    {
        get
        {
            return m_BottomOffest;
        }
    }
    Timer _caculateDelta;

    GameRangeAttribute _hp;
    GameRangeAttribute _mp;
    GameRangeAttribute _shield;
    GameAttribute _attack;
    GameAttribute _moveSpeed;

    protected GameRangeAttributeInstance m_Hp;
    protected GameRangeAttributeInstance m_Mp;
    protected GameRangeAttributeInstance m_Shield;
    protected GameAttributeInstance m_Attack;
    protected GameAttributeInstance m_MoveSpeed;

    protected override void OnInit(params object[] objs)
    {
        base.OnInit(objs);

        m_Animator = GetComponent<Animator>();
        Rigibody = GetComponent<Rigidbody2D>();
        m_BoxCollider = GetComponent<BoxCollider2D>();

        m_TopOffest = new Vector2(0, m_BoxCollider.size.y * 0.5f);
        m_BottomOffest = new Vector2(0, -m_BoxCollider.size.y * 0.5f + m_BoxCollider.offset.y);

        RegistAttribute();
        RegistFsmStatus();
        Reborn();
    }

    void RegistAttribute()
    {
        RoleConfig roleCfg = RoleConfig.GetData(Id);
        gameObject.name = roleCfg.Name;
        _hp = new GameRangeAttribute(E_Attribute.Hp.ToString(), 0, roleCfg.Hp);
        m_Hp = AddRangeAttribute(_hp);
        _mp = new GameRangeAttribute(E_Attribute.Mp.ToString(), 0, roleCfg.Mp, 0.1f);
        m_Mp = AddRangeAttribute(_mp);
        _attack = new GameAttribute(E_Attribute.Atk.ToString(), roleCfg.Attack);
        m_Attack = AddAttribute(_attack);
        _moveSpeed = new GameAttribute(E_Attribute.MoveSpeed.ToString(), roleCfg.MoveSpeed);
        m_MoveSpeed = AddAttribute(_moveSpeed);
        _shield = new GameRangeAttribute(E_Attribute.Shield.ToString(), 0, roleCfg.HitFlyShield, 5);
        m_Shield = AddRangeAttribute(_shield);

        _caculateDelta = TimerManager.Instance.AddListener(0, 0.02f, m_MonoAttribute.CaculateRangeDelta, null, true);

        RoleType = roleCfg.RoleType;

        switch (RoleType)
        {
            case RoleType.Player:
                UIBattleWindow.OnPlayerHpChange(m_Hp.Current, m_Hp.GetMinTotalValue(), m_Hp.GetMaxTotalValue());
                UIBattleWindow.OnPlayerMpChange(m_Mp.Current, m_Mp.GetMinTotalValue(), m_Mp.GetMaxTotalValue());
                GetRangeAttribute(E_Attribute.Hp.ToString()).OnValueChanged += (delta) => { UIBattleWindow.OnPlayerHpChange(m_Hp.Current, m_Hp.GetMinTotalValue(), m_Hp.GetMaxTotalValue()); };
                GetRangeAttribute(E_Attribute.Mp.ToString()).OnValueChanged += (delta) => { UIBattleWindow.OnPlayerMpChange(m_Mp.Current, m_Mp.GetMinTotalValue(), m_Mp.GetMaxTotalValue()); };
                break;
            case RoleType.Boss:
                UIBattleWindow.ShowBossUI();
                UIBattleWindow.OnBossHpChange(m_Hp.Current, m_Hp.GetMinTotalValue(), m_Hp.GetMaxTotalValue());
                UIBattleWindow.OnBossShieldChange(m_Shield.Current, m_Shield.GetMinTotalValue(), m_Shield.GetMaxTotalValue());
                GetRangeAttribute(E_Attribute.Hp.ToString()).OnValueChanged += (delta) => { UIBattleWindow.OnBossHpChange(m_Hp.Current, m_Hp.GetMinTotalValue(), m_Hp.GetMaxTotalValue()); };
                GetRangeAttribute(E_Attribute.Shield.ToString()).OnValueChanged += (delta) => { UIBattleWindow.OnBossShieldChange(m_Shield.Current, m_Shield.GetMinTotalValue(), m_Shield.GetMaxTotalValue()); };
                break;
        }
    }

    /// <summary>
    /// 注册角色状态
    /// </summary>
    void RegistFsmStatus()
    {
        CharIdleStatus idleStatus = new CharIdleStatus(this, m_Animator);
        fsm.AddStatus((int)E_CharacterFsmStatus.Idle, idleStatus);
        CharMoveStatus moveStatus = new CharMoveStatus(this, m_Animator);
        fsm.AddStatus((int)E_CharacterFsmStatus.Move, moveStatus);
        CharJumpStatus jumpStatus = new CharJumpStatus(this, m_Animator);
        fsm.AddStatus((int)E_CharacterFsmStatus.Jump, jumpStatus);
        CharAttackStatus attackStatus = new CharAttackStatus(this, m_Animator);
        fsm.AddStatus((int)E_CharacterFsmStatus.Attack, attackStatus);
        CharHurtStatus hurtStatus = new CharHurtStatus(this, m_Animator);
        fsm.AddStatus((int)E_CharacterFsmStatus.Hurt, hurtStatus);
        CharHitFlyStatus hitFlyStatus = new CharHitFlyStatus(this, m_Animator);
        fsm.AddStatus((int)E_CharacterFsmStatus.HitFly, hitFlyStatus);
        CharBlinkStatus blinkStatus = new CharBlinkStatus(this, m_Animator);
        fsm.AddStatus((int)E_CharacterFsmStatus.Blink, blinkStatus);
        CharPlayStatus playStatus = new CharPlayStatus(this, m_Animator);
        fsm.AddStatus((int)E_CharacterFsmStatus.Play, playStatus);
        CharBornStatus bornStatus = new CharBornStatus(this, m_Animator);
        fsm.AddStatus((int)E_CharacterFsmStatus.Born, bornStatus);
        CharDieStatus dieStatus = new CharDieStatus(this, m_Animator);
        fsm.AddStatus((int)E_CharacterFsmStatus.Die, dieStatus);

        ChangeStatus(E_CharacterFsmStatus.Idle);
    }

    /// <summary>
    /// 相当于Update
    /// </summary>
    protected override void OnUpdate()
    {
        base.OnUpdate();
        IsGround = Physics2D.OverlapCircle((Vector2)transform.position + m_BottomOffest, 0.3f, GameConfig.Instance.PlaneMask);
        if (!IsGround)
        {
            if (CurStatus != E_CharacterFsmStatus.Jump && CheckCanChangeStatus(E_CharacterFsmStatus.Jump))
            {
                System.Action<Character> act = SceneConfigManager.Instance.PlayJumpDownEffect;
                ChangeStatus(E_CharacterFsmStatus.Jump, false, 0, act);
            }
        }
        fsm.OnStay();
    }

    /// <summary>
    /// 检测是否可以切换到指定状态
    /// </summary>
    /// <param name="targetStatus"></param>
    /// <param name="beForce"></param>
    /// <returns></returns>
    public bool CheckCanChangeStatus(E_CharacterFsmStatus targetStatus, bool beForce = false)
    {
        return fsm.CheckCanChangeStatus((int)targetStatus, beForce);
    }

    /// <summary>
    /// 切换角色状态
    /// </summary>
    /// <param name="status">状态类型</param>
    /// <param name="beForce">是否强制切换</param>
    /// <returns>切换是否成功</returns>
    public bool ChangeStatus(E_CharacterFsmStatus status, bool beForce = false, params object[] objs)
    {
        bool result = fsm.ChangeStatus((int)status, beForce, objs);
        if (result)
        {
            CurStatus = status;
        }
        return result;
    }

    /// <summary>
    /// 是否可以中断当前状态
    /// </summary>
    /// <returns></returns>
    public bool CanInterruptStatus()
    {
        return fsm.GetStatus((int)CurStatus).CanInterrupt();
    }

    /// <summary>
    /// 复活
    /// </summary>
    void Reborn()
    {
        ResetAttributes();
        ChangeStatus(E_CharacterFsmStatus.Idle);
    }

    /// <summary>
    /// 是否可以连击
    /// </summary>
    /// <param name="skillId"></param>
    /// <returns></returns>
    public bool CanCombo(int skillId)
    {
        if (CurStatus == E_CharacterFsmStatus.Attack)
        {
            if (fsm.GetStatus((int)CurStatus).CanInterrupt())
            {
                return IsSubSkill(skillId);
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    /// <summary>
    /// 是否是当前技能的连击技
    /// </summary>
    /// <param name="skillId"></param>
    bool IsSubSkill(int skillId)
    {
        var skill = SkillConfig.GetData(skillId);
        while (skill.NextSkillId != 0)
        {
            if (skill.Id == CurSkill.Id)
            {
                return true;
            }
            skill = SkillConfig.GetData(skill.NextSkillId);
        }
        return false;
    }

    public override void OnDestory()
    {
        base.OnDestory();
        switch (RoleType)
        {
            case RoleType.Boss:
                UIBattleWindow.HideBossUI();
                break;
            case RoleType.Player:
                //LoadSceneManager.Instance.LoadScene(PlayerData.Instance.CurPlayerInfo.CurLevelId, Reborn);
                return;
        }
        TimerManager.Instance.RemoveListener(_caculateDelta);
    }

    protected override void OnDrawGizmosUpdate()
    {
        base.OnDrawGizmosUpdate();
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + m_BottomOffest, 0.3f);
    }

    #region 继承方法


    #endregion
}

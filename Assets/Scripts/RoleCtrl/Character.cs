using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(BehaviourTree))]
public class Character : MonoEntity
{
    [Header("角色当前状态")]
    public E_CharacterFsmStatus CurStatus;
    [Header("是否在地面上")]
    public bool IsGround;
    [Header("是否无敌")]
    public bool IsInvincible;
    [Header("攻击目标")]
    public Character AttackTarget;

    protected Animator m_Animator;
    protected Rigidbody2D m_Rigibody;
    protected BoxCollider2D m_BoxColider;

    FsmManager fsm = new FsmManager();
    protected int m_CurSkillId;
    protected Vector2 m_TopOffest;
    protected Vector2 m_BottomOffest;
    protected override void OnInit(params object[] objs)
    {
        base.OnInit(objs);

        m_Animator = GetComponent<Animator>();
        m_Rigibody = GetComponent<Rigidbody2D>();
        m_BoxColider = GetComponent<BoxCollider2D>();

        m_TopOffest = new Vector2(0, m_BoxColider.size.y * 0.5f);
        m_BottomOffest = new Vector2(0, -m_BoxColider.size.y * 0.5f);

        RegistFsmStatus();
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
        CharBlinkStatus blinkStatus = new CharBlinkStatus(this, m_Animator);
        fsm.AddStatus((int)E_CharacterFsmStatus.Blink, blinkStatus);
        CharPlayStatus playStatus = new CharPlayStatus(this, m_Animator);
        fsm.AddStatus((int)E_CharacterFsmStatus.Play, playStatus);
        CharBornStatus bornStatus = new CharBornStatus(this, m_Animator);
        fsm.AddStatus((int)E_CharacterFsmStatus.Born, bornStatus);
        CharDieStatus dieStatus = new CharDieStatus(this, m_Animator);
        fsm.AddStatus((int)E_CharacterFsmStatus.Die, dieStatus);
    }

    /// <summary>
    /// 相当于Update
    /// </summary>
    protected override void OnUpdate()
    {
        base.OnUpdate();
        fsm.OnStay();
        IsGround = Physics2D.OverlapCircle((Vector2)transform.position + m_BottomOffest, 0.3f, GameConfig.Instance.Plane);
    }

    /// <summary>
    /// 切换角色状态
    /// </summary>
    /// <param name="status">状态类型</param>
    /// <param name="beForce">是否强制切换</param>
    /// <returns>切换是否成功</returns>
    protected bool ChangeStatus(E_CharacterFsmStatus status, bool beForce = false, params object[] objs)
    {
        bool result = fsm.ChangeStatus((int)status, beForce, objs);
        if (result)
        {
            CurStatus = status;
        }
        return result;
    }

    /// <summary>
    /// 播放动画
    /// </summary>
    /// <param name="animIndex"></param>
    /// <param name="beForce"></param>
    public void PlayAnim(int animIndex, bool beForce = false)
    {
        ChangeStatus(E_CharacterFsmStatus.Play, beForce, animIndex);
    }

    #region 继承方法

    /// <summary>
    /// 重生
    /// </summary>
    protected virtual void ReBorn()
    {

    }

    /// <summary>
    /// 攻击
    /// </summary>
    /// <param name="skillId">技能Id</param>
    public virtual void Attack(int skillId, bool beForce = false)
    {

    }

    /// <summary>
    /// 受击
    /// </summary>
    public virtual void Hurt(GameObject atkOwner, int damage, bool beForce = false)
    {

    }

    /// <summary>
    /// 移动到指定点
    /// </summary>
    /// <param name="targetPos"></param>
    protected virtual void MoveToPoint(Vector3 targetPos)
    {

    }

    #endregion
}

using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class FsmBase
{
    public Action OnComplete;
    /// <summary>
    /// 可以进入
    /// </summary>
    /// <returns></returns>
    abstract public bool CanEnter();
    /// <summary>
    /// 可以中断
    /// </summary>
    /// <returns></returns>
    abstract public bool CanInterrupt();
    public void Enter()
    {
        OnEnter();
    }
    public void Stay()
    {
        OnStay();
    }
    public void Interrupt()
    {
        OnInterrupt();
    }
    public void Exit()
    {
        OnComplete?.Invoke();
        OnExit();
    }

    /// <summary>
    /// 进入时调用
    /// </summary>
    protected virtual void OnEnter()
    {
    }
    /// <summary>
    /// 运行时持续调用
    /// </summary>
    protected virtual void OnStay()
    {
    }
    /// <summary>
    /// 中断时调用
    /// </summary>
    protected virtual void OnInterrupt()
    {

    }
    /// <summary>
    /// 退出时调用
    /// </summary>
    protected virtual void OnExit()
    {
    }
}

public class FsmManager
{
    Dictionary<int, FsmBase> fsm;
    private int currentState = -1;
    public int CurrentState
    {
        get { return currentState; }
        private set { currentState = value; }
    }
    public FsmManager()
    {
        fsm = new Dictionary<int, FsmBase>();
    }
    public void AddState(int stateType, FsmBase fsmBase)
    {
        fsm.Add(stateType, fsmBase);
    }
    public FsmBase GetState(int stateType)
    {
        return fsm[stateType];
    }
    public bool CanChange(int state)
    {
        if (CurrentState == -1 && fsm[state].CanEnter()) return true;
        return fsm[CurrentState].CanInterrupt() && fsm[state].CanEnter();
    }

    public void OnStay()
    {
        if (fsm != null)
        {
            if (CurrentState != -1)
                fsm[CurrentState].Stay();
        }
    }

    public void ChangeState(int state, bool beForce = false)
    {
        if (!beForce&& !CanChange(state)) return;

        if (CurrentState != -1)
        {
            fsm[CurrentState].Interrupt();
        }

        CurrentState = state;
        fsm[CurrentState].Enter();
    }
}

public enum E_FsmState
{
    Idle,
    Move,
    Jump,
    Attack,
    Hurt,
    HitFly,
    Blink,
    Play,

    Born,
    Die,
    
    Max
}
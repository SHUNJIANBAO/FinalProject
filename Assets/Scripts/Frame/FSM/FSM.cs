﻿using System.Collections.Generic;
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
    /// <summary>
    /// 可以退出
    /// </summary>
    /// <returns></returns>
    abstract public bool CanExit();
    public void Enter(params object[] objs)
    {
        OnEnter(objs);
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
    protected virtual void OnEnter(params object[] objs)
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
    private int currentStatus = -1;
    public int CurrentStatus
    {
        get { return currentStatus; }
        private set { currentStatus = value; }
    }
    public FsmManager()
    {
        fsm = new Dictionary<int, FsmBase>();
    }
    public void AddStatus(int statusType, FsmBase fsmBase)
    {
        fsm.Add(statusType, fsmBase);
    }
    public FsmBase GetStatus(int statusType)
    {
        return fsm[statusType];
    }

    public void OnStay()
    {
        if (fsm != null)
        {
            if (CurrentStatus != -1)
                fsm[CurrentStatus].Stay();
        }
    }

    public bool ChangeStatus(int status, bool beForce = false, params object[] objs)
    {
        if (CurrentStatus!=-1)
        {
            if (beForce)
            {
                if (fsm[CurrentStatus].CanExit())
                {
                    fsm[CurrentStatus].Exit();
                }
                else if (fsm[CurrentStatus].CanInterrupt())
                {
                    fsm[CurrentStatus].Interrupt();
                }
                else
                    return false;
            }
            else
            {
                if (fsm[CurrentStatus].CanExit())
                {
                    fsm[CurrentStatus].Exit();
                }
                else
                    return false;
            }
        }

        CurrentStatus = status;
        fsm[CurrentStatus].Enter(objs);
        return true;
    }
}

public enum E_CharacterFsmStatus
{
    Idle,
    Move,
    Jump,
    Attack,
    Hurt,
    Blink,
    Play,

    Born,
    Die,
    
    Max
}
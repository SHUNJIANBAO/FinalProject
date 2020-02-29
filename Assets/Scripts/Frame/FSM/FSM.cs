using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class FsmBase
{
    abstract public bool CanEnter();
    abstract public bool CanExit();
    public virtual void OnEnter()
    {
    }
    public virtual void OnStay()
    {
    }
    public virtual void OnExit()
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
    public bool CanChange()
    {
        if (CurrentState == -1) return true;
        return fsm[CurrentState].CanExit();
    }

    public void OnStay()
    {
        if (fsm != null)
        {
            if (CurrentState != -1)
                fsm[CurrentState].OnStay();
        }
    }

    public bool CanEnter(int state)
    {
        return fsm[CurrentState].CanExit();
    }

    public void ChangeState(int state, bool beForce = false)
    {
        if (CurrentState != -1)
        {
            if (!fsm[CurrentState].CanExit() && !beForce)
            {
                return;
            }
            fsm[CurrentState].OnExit();
        }
        CurrentState = state;
        fsm[CurrentState].OnEnter();
    }
}
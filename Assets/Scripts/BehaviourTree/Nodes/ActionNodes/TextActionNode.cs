using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Node("行为/测试")]
public class TextActionNode : ActionNode
{
    public float abs;
    [System.NonSerialized]
    float timeCount;
    public override void Init()
    {
        Debug.Log(timeCount);
        Debug.Log("Init");
    }

    public override void OnEnter()
    {
        Debug.Log("Start");
    }

    public override void OnExit()
    {
        Debug.Log("Exit");
        timeCount = 0;
    }

    public override void OnStay()
    {
        Debug.Log("Stay");
        timeCount += Time.deltaTime;
    }

    protected override E_NodeStatus Trick()
    {
        Debug.Log("Trick");
        if (timeCount > abs)
        {
            return E_NodeStatus.Success;
        }

        return E_NodeStatus.Running;
    }
}

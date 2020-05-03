using UnityEngine;

[Node("等待节点")]
public class WaitNode : DecoratorNode
{
    public float WaitTime;

    [System.NonSerialized]
    float _enterTime;

    protected override void OnFirstEnter()
    {
        base.OnFirstEnter();
        _enterTime = Time.time;
    }

    protected override bool Condition()
    {
        return Time.time - _enterTime < WaitTime;
    }
}

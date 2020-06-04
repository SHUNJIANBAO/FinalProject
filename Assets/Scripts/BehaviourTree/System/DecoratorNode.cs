
public abstract class DecoratorNode : NodeBase
{
    bool _firstEnter = true;
#if UNITY_EDITOR
    public override bool CanConnectLineAsParent()
    {
        return ChildList.Count == 0;
    }
#endif

    protected virtual void OnFirstEnter() { }

    abstract protected bool Condition();

    protected override E_NodeStatus Trick()
    {
        if (_firstEnter)
        {
            _firstEnter = false;
            OnFirstEnter();
        }
        if (Condition())
        {
            return ChildList[0].GetTrick();
        }
        else
        {
            return E_NodeStatus.Failure;
        }
    }

    public override void OnComplete()
    {
        base.OnComplete();
        ChildList[0].OnComplete();
        _firstEnter = true;
    }
}

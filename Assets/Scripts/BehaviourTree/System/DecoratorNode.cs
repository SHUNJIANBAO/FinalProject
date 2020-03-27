
public abstract class DecoratorNode : NodeBase
{
    public override bool CanConnectLineAsParent()
    {
        return ChildList.Count == 0;
    }

    abstract protected bool Condition();

    protected override E_NodeStatus Trick()
    {
        if (Condition())
        {
            return ChildList[0].GetTrick();
        }
        else
        {
            return E_NodeStatus.Failure;
        }
    }
}

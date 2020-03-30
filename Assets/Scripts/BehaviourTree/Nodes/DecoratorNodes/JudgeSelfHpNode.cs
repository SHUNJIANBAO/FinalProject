
[Node("判断自身属性")]
public class JudgeSelfHpNode : DecoratorNode
{
    public E_JudgeCondition ConditionType;
    public E_Attribute AttributyType;
    public float Value;
    [System.NonSerialized]
    float _curValue;

    protected override bool Condition()
    {
        if (m_Owner.AttackTarget == null) return false;
        _curValue = m_Owner.GetAttribute(AttributyType.ToString()).GetTotalValue();
        switch (ConditionType)
        {
            case E_JudgeCondition.Less:
                return _curValue < Value;
            case E_JudgeCondition.Greater:
                return _curValue > Value;
            case E_JudgeCondition.Equals:
                return _curValue == Value;
            case E_JudgeCondition.NotEqual:
                return _curValue != Value;
        }
        throw new System.Exception("未知错误，无法进行距离判断");
    }
}

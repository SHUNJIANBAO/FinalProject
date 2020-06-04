using UnityEngine;

[Node("判断与目标距离")]
public class JudgeAttackTargetDistanceNode : DecoratorNode
{
    public E_JudgeCondition ConditionType;
    public float Horizontal;
    public float Vertical;
    //[System.NonSerialized]
    //float _curDistance;

    protected override bool Condition()
    {
        if (m_Owner.AttackTarget == null) return false;
        //_curDistance = Vector3.Distance(m_Owner.AttackTarget.transform.position, m_Owner.transform.position);
        switch (ConditionType)
        {
            case E_JudgeCondition.Less:
                return Mathf.Abs(m_Owner.transform.position.x - m_Owner.AttackTarget.transform.position.x) < Horizontal && Mathf.Abs(m_Owner.transform.position.y - m_Owner.AttackTarget.transform.position.y) < Vertical;
            case E_JudgeCondition.Greater:
                return Mathf.Abs(m_Owner.transform.position.x - m_Owner.AttackTarget.transform.position.x) > Horizontal && Mathf.Abs(m_Owner.transform.position.y - m_Owner.AttackTarget.transform.position.y) > Vertical;
            case E_JudgeCondition.Equals:
                return Mathf.Abs(m_Owner.transform.position.x - m_Owner.AttackTarget.transform.position.x) == Horizontal && Mathf.Abs(m_Owner.transform.position.y - m_Owner.AttackTarget.transform.position.y) == Vertical;
            case E_JudgeCondition.NotEqual:
                return Mathf.Abs(m_Owner.transform.position.x - m_Owner.AttackTarget.transform.position.x) != Horizontal && Mathf.Abs(m_Owner.transform.position.y - m_Owner.AttackTarget.transform.position.y) != Vertical;
        }
        throw new System.Exception("未知错误，无法进行距离判断");
    }
}

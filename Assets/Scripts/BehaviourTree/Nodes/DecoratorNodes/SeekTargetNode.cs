using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Node("搜索敌人")]
public class SeekTargetNode : DecoratorNode
{
    public RoleType TargetType = RoleType.Player;
    public float HorizontalDistance;
    public float VerticalDistance;

    Vector2 _size;
    LayerMask _targetMask;
    public override void Init()
    {
        base.Init();
        _size = new Vector2(HorizontalDistance*2, VerticalDistance*2);
        switch (TargetType)
        {
            case RoleType.Player:
                _targetMask = GameConfig.Instance.PlayerMask;
                break;
            case RoleType.Enemy:
                _targetMask = GameConfig.Instance.EnemyMask;
                break;
            case RoleType.Boss:
                _targetMask = GameConfig.Instance.EnemyMask;
                break;
        }
    }
    protected override bool Condition()
    {
        var hit = Physics2D.BoxCast(m_Owner.transform.position, _size, 0, Vector2.zero, 0, _targetMask);
        if (m_Owner.AttackTarget != null && hit.transform == m_Owner.AttackTarget.transform)
        {
            return true;
        }
        if (hit)
        {
            var character = hit.transform.GetComponent<Character>();
            if (character != null)
            {
                m_Owner.AttackTarget = character;
                return true;
            }
        }
        return false;
    }
}

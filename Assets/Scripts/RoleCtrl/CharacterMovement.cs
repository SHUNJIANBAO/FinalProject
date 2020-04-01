using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    Character _character;

    private void Awake()
    {
        _character = GetComponent<Character>();
    }

    public void Attack(int skillId,bool beForce)
    {

    }

    /// <summary>
    /// 播放动画
    /// </summary>
    /// <param name="animIndex"></param>
    /// <param name="beForce"></param>
    public void PlayAnim(int animIndex, bool beForce = false)
    {
        _character.ChangeStatus(E_CharacterFsmStatus.Play, beForce, animIndex);
    }

    /// <summary>
    /// 移动到指定点
    /// </summary>
    /// <param name="targetPos"></param>
    public void MoveToPoint(Vector3 targetPos)
    {
        _character.MoveTarget = targetPos;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, _character.GetAttribute(E_Attribute.MoveSpeed.ToString()).GetTotalValue()*GameManager.DeltaTime);
    }

    /// <summary>
    /// 重生
    /// </summary>
    protected virtual void ReBorn()
    {
        _character.ResetAttributes();
    }

    /// <summary>
    /// 受击
    /// </summary>
    public virtual void Hurt(GameObject atkOwner, int damage, bool beForce = false)
    {

    }

}

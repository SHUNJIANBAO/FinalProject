using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoEntity : MonoBehaviour 
{
    /// <summary>
    /// 排序下标，越小越先执行
    /// </summary>
    public int Index=1;
    //属性
    MonoAttribute m_MonoAttribute;

    /// <summary>
    /// 初始化方法（包括重置）
    /// </summary>
    public void Init()
    {
        OnInit();
    }

    protected virtual void OnInit()
    {

    }

    protected virtual void OnUpdate()
    {

    }
    public void CellUpdate()
    {
        if (!gameObject.activeSelf) return;
        OnUpdate();
    }

    protected virtual void OnFixedUpdate()
    {

    }
    public void CellFixedUpdate()
    {
        if (!gameObject.activeSelf) return;
        OnFixedUpdate();
    }

    protected virtual void OnLateUpdate()
    {
        
    }
    public void CellLateUpdate()
    {
        if (!gameObject.activeSelf) return;
        OnLateUpdate();
    }

    public void OnDrawGizmos()
    {
        OnDrawGizmosUpdate();
    }
    protected virtual void OnDrawGizmosUpdate() { }

    #region 属性相关

    /// <summary>
    /// 添加属性
    /// </summary>
    /// <param name="attribute"></param>
    /// <returns></returns>
    public GameAttributeInstance AddAttribute(GameAttribute attribute)
    {
        var attributeInstance= m_MonoAttribute.AddAttribute(attribute);
        return attributeInstance;
    }

    /// <summary>
    /// 添加有最大最小值的属性
    /// </summary>
    /// <param name="attribute"></param>
    /// <returns></returns>
    public GameRangeAttributeInstance AddRangeAttribute(GameRangeAttribute attribute)
    {
        var rangeAttributeInstance = m_MonoAttribute.AddRangeAttribute(attribute);
        return rangeAttributeInstance;
    }

    /// <summary>
    /// 得到属性
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public GameAttributeInstance GetAttribute(string name)
    {
       return m_MonoAttribute.GetAttribute(name);
    }
    /// <summary>
    /// 得到有最大最小值的属性
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public GameRangeAttributeInstance GetRangeAttribute(string name)
    {
        return m_MonoAttribute.GetRangeAttribute(name);
    }

    /// <summary>
    /// 增加属性加成
    /// </summary>
    /// <param name="attribute"></param>
    /// <param name="modifier"></param>
    public void AddModifier(GameAttributeInstance attribute,GameAttributeModifier modifier)
    {
        attribute.AddModifier(modifier);
    }
    /// <summary>
    /// 增加属性最小值加成
    /// </summary>
    /// <param name="attribute"></param>
    /// <param name="modifier"></param>
    public void AddMinModifier(GameRangeAttributeInstance attribute, GameAttributeModifier modifier)
    {
        attribute.AddMinModifier(modifier);
    }
    /// <summary>
    /// 增加属性最大值加成
    /// </summary>
    /// <param name="attribute"></param>
    /// <param name="modifier"></param>
    public void AddMaxModifier(GameRangeAttributeInstance attribute, GameAttributeModifier modifier)
    {
        attribute.AddMaxModifier(modifier);
    }
    /// <summary>
    /// 增加属性回复值加成
    /// </summary>
    /// <param name="attribute"></param>
    /// <param name="modifier"></param>
    public void AddDeltaModifier(GameRangeAttributeInstance attribute, GameAttributeModifier modifier)
    {
        attribute.AddDeltaModifier(modifier);
    }

    /// <summary>
    /// 移除属性加成
    /// </summary>
    /// <param name="attribute"></param>
    /// <param name="modifier"></param>
    public void RemoveModifier(GameAttributeInstance attribute, GameAttributeModifier modifier)
    {
        attribute.RemoveModifier(modifier);
    }
    /// <summary>
    /// 移除属性最小值加成
    /// </summary>
    /// <param name="attribute"></param>
    /// <param name="modifier"></param>
    public void RemoveMinModifier(GameRangeAttributeInstance attribute, GameAttributeModifier modifier)
    {
        attribute.RemoveMinModifier(modifier);
    }
    /// <summary>
    /// 移除属性最大值加成
    /// </summary>
    /// <param name="attribute"></param>
    /// <param name="modifier"></param>
    public void RemoveMaxModifier(GameRangeAttributeInstance attribute, GameAttributeModifier modifier)
    {
        attribute.RemoveMaxModifier(modifier);
    }
    /// <summary>
    /// 移除属性回复值加成
    /// </summary>
    /// <param name="attribute"></param>
    /// <param name="modifier"></param>
    public void RemoveDeltaModifier(GameRangeAttributeInstance attribute, GameAttributeModifier modifier)
    {
        attribute.RemoveDeltaModifier(modifier);
    }

    /// <summary>
    /// 清空属性加成
    /// </summary>
    /// <param name="attribute"></param>
    /// <param name="modifier"></param>
    public void ClearModifier(GameAttributeInstance attribute)
    {
        attribute.ClearModifier();
    }
    /// <summary>
    /// 清空属性最小值加成
    /// </summary>
    /// <param name="attribute"></param>
    /// <param name="modifier"></param>
    public void ClearMinModifier(GameRangeAttributeInstance attribute)
    {
        attribute.ClearMinModifier();
    }
    /// <summary>
    /// 清空属性最大值加成
    /// </summary>
    /// <param name="attribute"></param>
    /// <param name="modifier"></param>
    public void ClearMaxModifier(GameRangeAttributeInstance attribute)
    {
        attribute.ClearMaxModifier();
    }
    /// <summary>
    /// 清空属性回复值加成
    /// </summary>
    /// <param name="attribute"></param>
    /// <param name="modifier"></param>
    public void ClearDeltaModifier(GameRangeAttributeInstance attribute)
    {
        attribute.ClearDeltaModifier();
    }
    #endregion
}

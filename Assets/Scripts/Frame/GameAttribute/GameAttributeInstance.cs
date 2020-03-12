using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAttributeInstance 
{
    public string Name => this.m_GameAttribute.Name;
    public MonoEntity m_MonoCell { get; private set; }
    GameAttribute m_GameAttribute;
    List<GameAttributeModifier> m_GameAttributeModifiersList;  //附加值

    public GameAttributeInstance(MonoEntity monoCell,GameAttribute attribute)
    {
        this.m_MonoCell = monoCell;
        this.m_GameAttribute = attribute;
        m_GameAttributeModifiersList = new List<GameAttributeModifier>();
    }

    public void AddModifier(GameAttributeModifier modifier)
    {
        m_GameAttributeModifiersList.Add(modifier);
    }

    public void RemoveModifier(GameAttributeModifier modifier)
    {
        m_GameAttributeModifiersList.Remove(modifier);
    }

    public void ClearModifier()
    {
        m_GameAttributeModifiersList.Clear();
    }

    /// <summary>
    /// 得到基础数值
    /// </summary>
    public float GetBaseValue => this.m_GameAttribute.Value;

    /// <summary>
    /// 得到增加值,倍率之间是相加关系
    /// </summary>
    /// <returns></returns>
    public float GetModifierContribution(GameAttributeModifier modifier)
    {
        if (!modifier.IsMultiplier)
        {
            return modifier.Value;
        }
        float baseValue = GetBaseValue;
        for (int i = 0; i < m_GameAttributeModifiersList.Count; i++)
        {
            if (!m_GameAttributeModifiersList[i].IsMultiplier)
            {
                baseValue += m_GameAttributeModifiersList[i].Value;
            }
        }
        return baseValue * modifier.Value;
    }

    /// <summary>
    /// 得到加成后的总值
    /// </summary>
    /// <returns></returns>
    public float GetTotalValue()
    {
        float baseValue = GetBaseValue;
        float multiplier = 1;
        for (int i = 0; i < m_GameAttributeModifiersList.Count; i++)
        {
            if (m_GameAttributeModifiersList[i].IsMultiplier)
            {
                multiplier += m_GameAttributeModifiersList[i].Value;
            }
            else
            {
                baseValue+= m_GameAttributeModifiersList[i].Value;
            }
        }
        return baseValue * multiplier;
    }
}

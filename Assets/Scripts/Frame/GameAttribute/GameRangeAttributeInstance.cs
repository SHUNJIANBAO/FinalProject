using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameRangeAttributeInstance
{
    public string Name => this.m_GameRangeAttribute.Name;
    public MonoEntity m_MonoCell { get; private set; }
    GameRangeAttribute m_GameRangeAttribute;
    List<GameAttributeModifier> m_GameAttributeMinModifiersList;  //最小值附加值
    List<GameAttributeModifier> m_GameAttributeMaxModifiersList;  //最大值附加值
    List<GameAttributeModifier> m_GameAttributeDeltaModifiersList;  //回复值附加值
    public float Current;
    public Action<float> OnValueChanged;

    public GameRangeAttributeInstance(MonoEntity monoCell, GameRangeAttribute attribute)
    {
        this.m_MonoCell = monoCell;
        this.m_GameRangeAttribute = attribute;
        m_GameAttributeMinModifiersList = new List<GameAttributeModifier>();
        m_GameAttributeMaxModifiersList = new List<GameAttributeModifier>();
        m_GameAttributeDeltaModifiersList = new List<GameAttributeModifier>();
    }

    public void AddMinModifier(GameAttributeModifier modifier)
    {
        m_GameAttributeMinModifiersList.Add(modifier);
    }

    public void RemoveMinModifier(GameAttributeModifier modifier)
    {
        m_GameAttributeMinModifiersList.Remove(modifier);
    }

    public void AddMaxModifier(GameAttributeModifier modifier)
    {
        m_GameAttributeMaxModifiersList.Add(modifier);
    }

    public void RemoveMaxModifier(GameAttributeModifier modifier)
    {
        m_GameAttributeMaxModifiersList.Remove(modifier);
    }

    public void AddDeltaModifier(GameAttributeModifier modifier)
    {
        m_GameAttributeDeltaModifiersList.Add(modifier);
    }

    public void RemoveDeltaModifier(GameAttributeModifier modifier)
    {
        m_GameAttributeDeltaModifiersList.Remove(modifier);
    }

    public void ClearModifier()
    {
        m_GameAttributeMinModifiersList.Clear();
        m_GameAttributeMaxModifiersList.Clear();
        m_GameAttributeDeltaModifiersList.Clear();
    }

    /// <summary>
    /// 得到基础最小数值
    /// </summary>
    public float GetMinBaseValue => this.m_GameRangeAttribute.Min.Value;
    /// <summary>
    /// 得到基础最大数值
    /// </summary>
    public float GetMaxBaseValue => this.m_GameRangeAttribute.Max.Value;
    /// <summary>
    /// 得到基础回复值
    /// </summary>
    public float GetDeltaBaseValue => this.m_GameRangeAttribute.Delta.Value;

    public void SetValue(float value)
    {
        Current = value;
    }

    public void ChangeValue(float delta)
    {
        Current = Mathf.Clamp(Current + delta, GetMinTotalValue(), GetMaxTotalValue());
        OnValueChanged?.Invoke(delta);
    }

    /// <summary>
    /// 清空最小值附加
    /// </summary>
    public void ClearMinModifier()
    {
        m_GameAttributeMinModifiersList.Clear();
    }
    /// <summary>
    /// 清空最大值附加
    /// </summary>
    public void ClearMaxModifier()
    {
        m_GameAttributeMaxModifiersList.Clear();
    }
    /// <summary>
    /// 清空回复值附加
    /// </summary>
    public void ClearDeltaModifier()
    {
        m_GameAttributeDeltaModifiersList.Clear();
    }


    /// <summary>
    /// 得到最小值的附加值
    /// </summary>
    /// <param name="modifier"></param>
    /// <returns></returns>
    public float GetMinModifierContribution(GameAttributeModifier modifier)
    {
        if (!modifier.IsMultiplier)
        {
            return modifier.Value;
        }
        float baseValue = GetMinBaseValue;
        for (int i = 0; i < m_GameAttributeMinModifiersList.Count; i++)
        {
            if (!m_GameAttributeMinModifiersList[i].IsMultiplier)
            {
                baseValue += m_GameAttributeMinModifiersList[i].Value;
            }
        }
        return baseValue * modifier.Value - baseValue;
    }

    /// <summary>
    /// 得到最小值加成后的总值
    /// </summary>
    /// <returns></returns>
    public float GetMinTotalValue()
    {
        float baseValue = GetMinBaseValue;
        float multiplier = 1;
        for (int i = 0; i < m_GameAttributeMinModifiersList.Count; i++)
        {
            if (m_GameAttributeMinModifiersList[i].IsMultiplier)
            {
                multiplier += m_GameAttributeMinModifiersList[i].Value - 1;
            }
            else
            {
                baseValue += m_GameAttributeMinModifiersList[i].Value;
            }
        }
        return baseValue * multiplier;
    }


    /// <summary>
    /// 得到最大值附加值
    /// </summary>
    /// <param name="modifier"></param>
    /// <returns></returns>
    public float GetMaxModifierContribution(GameAttributeModifier modifier)
    {
        if (!modifier.IsMultiplier)
        {
            return modifier.Value;
        }
        float baseValue = GetMaxBaseValue;
        for (int i = 0; i < m_GameAttributeMaxModifiersList.Count; i++)
        {
            if (!m_GameAttributeMaxModifiersList[i].IsMultiplier)
            {
                baseValue += m_GameAttributeMaxModifiersList[i].Value;
            }
        }
        return baseValue * modifier.Value - baseValue;
    }

    /// <summary>
    /// 得到最大值加成后的总值
    /// </summary>
    /// <returns></returns>
    public float GetMaxTotalValue()
    {
        float baseValue = GetMaxBaseValue;
        float multiplier = 1;
        for (int i = 0; i < m_GameAttributeMaxModifiersList.Count; i++)
        {
            if (m_GameAttributeMaxModifiersList[i].IsMultiplier)
            {
                multiplier += m_GameAttributeMaxModifiersList[i].Value - 1;
            }
            else
            {
                baseValue += m_GameAttributeMaxModifiersList[i].Value;
            }
        }
        return baseValue * multiplier;
    }

    /// <summary>
    /// 得到回复值附加值
    /// </summary>
    /// <param name="modifier"></param>
    /// <returns></returns>
    public float GetDeltaModifierContribution(GameAttributeModifier modifier)
    {
        if (!modifier.IsMultiplier)
        {
            return modifier.Value;
        }
        float baseValue = GetDeltaBaseValue;
        for (int i = 0; i < m_GameAttributeDeltaModifiersList.Count; i++)
        {
            if (!m_GameAttributeDeltaModifiersList[i].IsMultiplier)
            {
                baseValue += m_GameAttributeDeltaModifiersList[i].Value;
            }
        }
        return baseValue * modifier.Value - baseValue;
    }

    /// <summary>
    /// 得到最大值加成后的总值
    /// </summary>
    /// <returns></returns>
    public float GetDeltaTotalValue()
    {
        float baseValue = GetDeltaBaseValue;
        float multiplier = 1;
        for (int i = 0; i < m_GameAttributeDeltaModifiersList.Count; i++)
        {
            if (m_GameAttributeDeltaModifiersList[i].IsMultiplier)
            {
                multiplier += m_GameAttributeDeltaModifiersList[i].Value - 1;
            }
            else
            {
                baseValue += m_GameAttributeDeltaModifiersList[i].Value;
            }
        }
        return baseValue * multiplier;
    }

    public void Reset()
    {
        ClearModifier();
        SetValue(GetMaxTotalValue());
    }
}

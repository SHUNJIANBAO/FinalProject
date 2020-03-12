using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoAttribute
{
    List<GameAttributeInstance> m_AttributesList;
    List<GameRangeAttributeInstance> m_RangeAttributesList;
    Dictionary<string, GameAttributeInstance> m_AttributeNameDict = new Dictionary<string, GameAttributeInstance>();
    Dictionary<string, GameRangeAttributeInstance> m_RangeAttributeNameDict = new Dictionary<string, GameRangeAttributeInstance>();
    public MonoEntity m_Owner { get; private set; }

    public MonoAttribute(MonoEntity entity)
    {
        this.m_Owner = entity;
        m_AttributesList = new List<GameAttributeInstance>();
        m_RangeAttributesList = new List<GameRangeAttributeInstance>();
    }

    public GameAttributeInstance AddAttribute(GameAttribute attribute)
    {
        GameAttributeInstance attr = new GameAttributeInstance(this.m_Owner, attribute);
        AddAttribute(attr);
        return attr;
    }

    public void AddAttribute(GameAttributeInstance attribute)
    {
        if (!m_AttributesList.Contains(attribute))
        {
            m_AttributesList.Add(attribute);
            m_AttributeNameDict.Add(attribute.Name, attribute);
        }
    }

    public GameRangeAttributeInstance AddRangeAttribute(GameRangeAttribute attribute)
    {
        GameRangeAttributeInstance attr = new GameRangeAttributeInstance(this.m_Owner, attribute);
        AddRangeAttribute(attr);
        return attr;
    }

    public void AddRangeAttribute(GameRangeAttributeInstance rangeAttribute)
    {
        if (!m_RangeAttributesList.Contains(rangeAttribute))
        {
            m_RangeAttributesList.Add(rangeAttribute);
            m_RangeAttributeNameDict.Add(rangeAttribute.Name, rangeAttribute);
        }
    }

    public GameAttributeInstance GetAttribute(string name)
    {
        //m_AttributeNameDict.TryGetValue(name, out GameAttribute attribute);
        if (m_AttributeNameDict.ContainsKey(name))
            return m_AttributeNameDict[name];
        return null;
    }

    public GameRangeAttributeInstance GetRangeAttribute(string name)
    {
        if (m_RangeAttributeNameDict.ContainsKey(name))
            return m_RangeAttributeNameDict[name];
        return null;
    }

    public void AddAttributeModifier(GameAttribute attribute, GameAttributeModifier modifier)
    {
        
    }

    public void AddRangeAttributeModifier(GameRangeAttributeInstance rangeAttribute,GameAttributeModifier modifier)
    {

    }

    public void AddAttributeModifier(string attributeName, GameAttributeModifier modifier)
    {

    }

    public void AddRangeAttributeModifier(string rangeAttribute, GameAttributeModifier modifier)
    {

    }

    public void AddAttributeModifier(GameAttributeModifier modifier)
    {

    }

    public void AddRangeAttributeModifier(GameAttributeModifier modifier)
    {

    }


    public void RemoveAttributeModifier(GameAttributeModifier modifier)
    {

    }

    public void RemoveRangeAttributeModifier(GameRangeAttributeInstance modifier)
    {

    }

}

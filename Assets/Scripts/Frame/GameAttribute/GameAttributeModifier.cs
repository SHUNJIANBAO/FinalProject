using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAttributeModifier 
{
    public string Name { get; private set; }
    public bool IsMultiplier { get; private set; } //是否倍数
    public float Value { get; private set; }

    public GameAttributeModifier(string name,float value,bool isMultiplier = false)
    {
        this.Name = name;
        this.Value = value;
        this.IsMultiplier = isMultiplier;
    }

    public void SetValue(float value)
    {
        this.Value = value;
    }
}

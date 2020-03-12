using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRangeAttribute
{
    public string Name;
    public string Description;
    public GameAttribute Min;
    public GameAttribute Max;
    public GameAttribute Delta;
    public GameRangeAttribute(string name,float minValue,float maxValue,float delta=0,string description="")
    {
        this.Name = name;
        this.Description = description;
        this.Min = new GameAttribute("Min_" + name, minValue);
        this.Max = new GameAttribute("Max_" + name, maxValue);
        this.Delta = new GameAttribute("Delta_" + name, delta);
    }
}

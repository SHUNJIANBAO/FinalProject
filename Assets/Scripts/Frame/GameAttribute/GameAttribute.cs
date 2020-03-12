
public enum E_Attribute
{
    HP,
    MP,
    ATK,
    DEF,
    MOVESPEED,
}

public class GameAttribute  
{
    public string Name;
    public float Value;
    public string Description;
    public GameAttribute(string name,float value,string description = "")
    {
        this.Name = name;
        this.Value = value;
        this.Description = description;
    }
}


public enum E_Attribute
{
    Hp,
    Mp,
    Atk,
    Def,
    MoveSpeed,
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

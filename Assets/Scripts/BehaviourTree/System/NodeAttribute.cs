using System;

public class NodeAttribute : Attribute 
{
    public string NodeName;
    public NodeAttribute(string nodeName)
    {
        this.NodeName = nodeName;
    }
}

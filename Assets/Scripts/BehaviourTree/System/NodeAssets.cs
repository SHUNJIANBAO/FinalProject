using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NodeAssets : ScriptableObject
{
    public NodeBase RootNode;
    public List<NodeBase> NodesList = new List<NodeBase>();
}

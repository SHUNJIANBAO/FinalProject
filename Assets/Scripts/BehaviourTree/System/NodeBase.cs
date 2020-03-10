using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeBase
{
#if UNITY_EDITOR
    Vector3 m_Pos;
    public Vector3 Pos
    {
        get
        {
            return m_Pos;
        }
        set
        {
            m_Pos = value;
        }
    }

#endif
}

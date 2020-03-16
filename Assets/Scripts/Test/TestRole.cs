using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRole : Character 
{
    private void Awake()
    {
        MonoBehaviourManager.Add(this);
        //Init();
    }
}

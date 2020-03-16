using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoEntity 
{
    FsmManager fsm = new FsmManager();

    protected override void OnInit()
    {
        base.OnInit();
        
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        fsm.OnStay();
    }
}

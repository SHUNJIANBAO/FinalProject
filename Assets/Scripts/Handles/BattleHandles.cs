using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleHandles  
{
    public static void HitFeel(MonoBehaviour mono,float hitStopTime)
    {
        if (!GameManager.IsTimeStop)
        {
            GameManager.StopOtherTime(null, true);
            Util.RunLater(mono, () => { GameManager.StopOtherTime(null, false); }, hitStopTime);
        }
        CameraManager.ShakeCamera();
    }
}

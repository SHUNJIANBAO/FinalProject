using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager 
{
    public static bool IsPause;
    public static bool IsLoading;
    public static float TimeRatio=1;
    public static float DeltaTime
    {
        get
        {
            return TimeRatio * Time.deltaTime;
        }
    }
}

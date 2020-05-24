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
    public static bool IsTimeStop;

    public static Player Player { get; set; }
    public static SceneConfigManager CurSceneManager { get; set; }

    private static int _curLevelId;
    public static int CurLevelId
    {
        get
        {
            return _curLevelId;
        }
        set
        {
            _curLevelId = value;
            var levelCfg = LevelConfig.GetData(_curLevelId);
            if (!string.IsNullOrEmpty(levelCfg.BGM))
            {
                AudioManager.Instance.PlayBGM(levelCfg.BGM);
            }

        }
    }

    public static void StopOtherTime(MonoEntity mono,bool value)
    {
        MonoBehaviourManager. SetOtherAnimatorSpeed(mono, value?0:1);
        IsTimeStop = value;
    }

    public static void SetCurSceneGray(bool value)
    {
        CurSceneManager.SetSceneGray(value);
    }
}

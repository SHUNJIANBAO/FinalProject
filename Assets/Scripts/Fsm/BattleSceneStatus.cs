using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BattleSceneStatus : Singleton<BattleSceneStatus>
{
    public void StartBattle()
    {
        PlayerData.Instance.StartTime = DateTime.Now;
        UIManager.Instance.OpenWindow<UIBattleWindow>();
    }

    public void EndBattle()
    {
        PlayerData.Instance.SavePlayerInfo(PlayerData.Instance.CurPlayerInfo);
        UIManager.Instance.CloseAllWindow();
        UIManager.Instance.OpenWindow<UIMainMenuWindow>();
    }
}

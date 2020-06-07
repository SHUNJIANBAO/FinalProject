using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class LevelStruct
{
    public int SaveId;
    public List<LevelInfo> LevelInfos;
}

[System.Serializable]
public class LevelInfo
{
    public int LevelId;
    public List<int> GetItemList;
    public bool OpenDoor;
}


public class LevelData : Data<LevelData>
{
    public List<LevelStruct> LevelStructs;
    protected override void OnLoad()
    {
        base.OnLoad();
        if (LevelStructs == null)
        {
            LevelStructs = new List<LevelStruct>();
        }
    }

    public void DelLevelData(int saveId)
    {
        for (int i = 0; i < LevelStructs.Count; i++)
        {
            if (LevelStructs[i].SaveId==saveId)
            {
                LevelStructs.RemoveAt(i);
                return;
            }
        }
    }

    /// <summary>
    /// 获取当前LevelInfo
    /// </summary>
    /// <returns></returns>
    LevelInfo FindCurLevel()
    {
        int saveId = PlayerData.Instance.CurPlayerInfo.SaveId;
        LevelStruct tempLevel = null;
        for (int i = 0; i < LevelStructs.Count; i++)
        {
            if (LevelStructs[i].SaveId == saveId)
            {
                tempLevel = LevelStructs[i];
                for (int j = 0; j < LevelStructs[i].LevelInfos.Count; j++)
                {
                    if (LevelStructs[i].LevelInfos[j].LevelId == GameManager.CurLevelId)
                    {
                        return LevelStructs[i].LevelInfos[j];
                    }
                }
            }
        }
        if (tempLevel == null)
        {
            LevelStruct level = new LevelStruct();
            level.SaveId = saveId;
            level.LevelInfos = new List<LevelInfo>();
            LevelInfo info = new LevelInfo();
            info.LevelId = GameManager.CurLevelId;
            info.GetItemList = new List<int>();
            level.LevelInfos.Add(info);
            LevelStructs.Add(level);
            return info;
        }
        else
        {
            LevelInfo info = new LevelInfo();
            info.LevelId = GameManager.CurLevelId;
            info.GetItemList = new List<int>();
            tempLevel.LevelInfos.Add(info);
            return info;
        }
    }

    public void OpenDoor()
    {
        var curInfo = FindCurLevel();
        curInfo.OpenDoor = true;
    }

    public bool DoorIsOpen()
    {
        var curInfo = FindCurLevel();
        return curInfo.OpenDoor;
    }

    public void GetItem(int itemId)
    {
        var curInfo = FindCurLevel();
        curInfo.GetItemList.Add(itemId);
    }

    public bool HasItem(int itemId)
    {
        var curInfo = FindCurLevel();
        return curInfo.GetItemList.Contains(itemId);
    }
}

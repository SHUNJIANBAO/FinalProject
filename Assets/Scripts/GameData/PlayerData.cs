using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class PlayerInfo
{
    public int SaveId;
    public int GameTimeMinutes;
    public List<ItemInfo> ItemList;
}

[System.Serializable]
public class ItemInfo
{
    public int ItemId;
    public int ItemCount;
    public ItemPos ItemPos;
}

public class PlayerData : Data<PlayerData>
{
    public List<PlayerInfo> PlayerInfos;
    public PlayerInfo CurPlayerInfo;

    public DateTime StartTime;

    protected override void OnLoad()
    {
        base.OnLoad();
        if (PlayerInfos==null)
        {
            PlayerInfos = new List<PlayerInfo>();
        }
    }

    /// <summary>
    /// 创建存档
    /// </summary>
    /// <param name="saveId"></param>
    public PlayerInfo CreatePlayerInfo(int saveId)
    {
        var info= PlayerInfos.Find(i => i.SaveId == saveId);
        if (info!=null)
        {
            PlayerInfos.Remove(info);
        }
        PlayerInfo newInfo = new PlayerInfo();
        newInfo.SaveId = saveId;
        PlayerInfos.Add(newInfo);
        Save();
        return newInfo;
    }

    public void SavePlayerInfo(PlayerInfo info)
    {
        var tempInfo = PlayerInfos.Find(i => i.SaveId == info.SaveId);
        if (tempInfo == null) throw new Exception("Archive Error!!!");
        tempInfo = info;
        tempInfo.GameTimeMinutes += (DateTime.Now - StartTime).Minutes;
        Save();
    }

    /// <summary>
    /// 删除存档
    /// </summary>
    /// <param name="info"></param>
    public void DeletePlayerInfo(PlayerInfo info)
    {
        PlayerInfos.Remove(info);
        Save();
    }

    /// <summary>
    /// 得到物品
    /// </summary>
    /// <param name="itemId"></param>
    public void GetItem(int itemId,int count,Action callback=null)
    {
        var itemInfo= CurPlayerInfo.ItemList.Find(info => info.ItemId == itemId);
        if (itemInfo==null)
        {
            ItemInfo info = new ItemInfo();
            info.ItemId = itemId;
            info.ItemCount = count;
            info.ItemPos = ItemPos.Package;
            CurPlayerInfo.ItemList.Add(info);
        }
        else
        {
            itemInfo.ItemCount += count;
        }
        if (callback != null)
        {
            callback();
        }
    }

    /// <summary>
    /// 丢弃物品
    /// </summary>
    /// <param name="itemId"></param>
    public void DiscardItem(int itemId, Action callback = null)
    {
        var item = CurPlayerInfo.ItemList.Find(info => info.ItemId == itemId);
        if (item!=null)
        {
            CurPlayerInfo.ItemList.Remove(item);
        }
        if (callback != null)
        {
            callback();
        }
    }

    /// <summary>
    /// 使用物品，是装备则装备
    /// </summary>
    /// <param name="itemId"></param>
    public void UseItem(int itemId, Action callback = null)
    {
        var item = CurPlayerInfo.ItemList.Find(info => info.ItemId == itemId);
        if (item != null)
        {
            var itemCfg = ItemConfig.GetData(itemId);
            switch (itemCfg.ItemType)
            {
                case ItemType.Consumable:
                    
                    break;
                case ItemType.Equipment:
                    break;
            }
            item.ItemPos = ItemPos.Use;
        }

        if (callback != null)
        {
            callback();
        }
    }

    /// <summary>
    /// 卸下物品
    /// </summary>
    /// <param name="itemId"></param>
    public void UnUseItem(int itemId, Action callback = null)
    {
        var item = CurPlayerInfo.ItemList.Find(info => info.ItemId == itemId);

        if (callback != null)
        {
            callback();
        }
    }

}

public enum ItemPos
{
    Package, //背包
    Use //装备中
}
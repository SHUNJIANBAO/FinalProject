using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Normal=0,  //普通物品
    Consumable=1,  //消耗品
    Equipment=2,  //装备
}

public class ItemConfig : CsvCfg<ItemConfig>
{
    public string Name { get; protected set; }
    public ItemType ItemType { get; protected set; }
    public string Description { get; protected set; }
    public string Icon { get; protected set; }
    public int MaxCount { get; protected set; }

    public List<int> TestList { get; protected set; }

    public static string FilePath = "Config/Item";
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumeableConfig : CsvCfg<ConsumeableConfig>
{
    public string Name { get; protected set; }
    public int AddBuff { get; protected set; }

    public static string FilePath = "Config/Consumeable";

}

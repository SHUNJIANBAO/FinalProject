using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelConfig : CsvCfg<LevelConfig>
{
    public string Name { get; protected set; }
    public string TitleIcon { get; protected set; }
    public string BGM { get; protected set; }

    public static string FilePath = "Config/Level";
}

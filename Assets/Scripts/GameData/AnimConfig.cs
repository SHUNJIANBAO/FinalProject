using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimConfig : CsvCfg<AnimConfig> 
{
    public string Name { get; protected set; }
    public static string FilePath = "Config/Anim";
}

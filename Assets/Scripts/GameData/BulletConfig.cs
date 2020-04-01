using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletConfig : CsvCfg<BulletConfig>
{
    public string Name { get; protected set; }

    public static string FilePath = "Config/Bullet";

}
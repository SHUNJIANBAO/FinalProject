using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager
{
    public static string GetWindowPath(string windowName)
    {
        return "Prefabs/UGUI/"+windowName+"/"+windowName;
    }

    public static string GetWindowAssetsPath(string windowName)
    {
        return "Assets/Resources/Prefabs/UGUI/" + windowName ;
    }

    public static string GetWindowScriptPath(string windowName)
    {
        return "Assets/Scripts/UGUI/"+windowName;
    }

    public const string AudioPath = "Audios";
}

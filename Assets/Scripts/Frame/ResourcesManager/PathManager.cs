using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager
{
    public static string GetWindowPath(string windowName)
    {
        return "Prefabs/UGUI/"+windowName+"/"+windowName;
    }

    public static string GetPanelPath(string windowName,string panelName)
    {
        return "Prefabs/UGUI/" + windowName + "/" + panelName;
    }

    public static string GetWindowAssetsPath(string windowName)
    {
        return "Assets/Resources/Prefabs/UGUI/" + windowName ;
    }

    public static string GetWindowScriptPath(string windowName)
    {
        return "Assets/Scripts/UGUI/"+windowName;
    }

    public static string GetEffectPath(string effName)
    {
        return "Prefabs/Effects/" + effName+"/"+effName;
    }

    public static string GetEmitterPath(string emitterName)
    {
        return "Prefabs/Emitters/" + emitterName;
    }

    public static string GetBulletPath(string bulletName)
    {
        return "Prefabs/Bullets/" + bulletName;
    }

    public static string ScenesPath = "Assets/Scenes/BattleScenes/";

    public static string ColliderPath = "Prefabs/Collider/Collider";

    public static string RolePrefabsPath = "Prefabs/Roles/";

    public const string AudioPath = "Audios";
}

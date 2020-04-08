using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ResourceManager
{
    static Dictionary<string, List<Object>> m_AllGameObjects = new Dictionary<string, List<Object>>();

    public static T Load<T>(string path)where T:Object
    {
        T obj = Resources.Load<T>(path);
        //m_AllGameObjects.Add(path, new List<Object> { obj });
        return obj;
    }

    public static List<Object> LoadAll(string path)
    {
        List<Object> objs = Resources.LoadAll<Object>(path).ToList();
        m_AllGameObjects.Add(path, objs);
        return objs;
    }

    public static Object GetGameObject(string path,string objName)
    {
        if (!m_AllGameObjects.ContainsKey(path))
        {
            Debug.LogError("Can't find:" + path);
        }
        return m_AllGameObjects[path].Find(obj => obj.name == objName);
    }

    public static List<Object> GetGameObjectList(string path)
    {
        return m_AllGameObjects[path];
    }

    /// <summary>
    /// 释放资源
    /// </summary>
    /// <param name="path"></param>
    public static void Release(string path)
    {
        m_AllGameObjects.Remove(path);
    }

}

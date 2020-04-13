using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ResourceManager
{
    static Dictionary<string, List<Object>> m_AllGameObjects = new Dictionary<string, List<Object>>();
    static Dictionary<string, Object> m_GameObjects = new Dictionary<string, Object>();

    public static T Load<T>(string path)where T:Object
    {
        if (m_GameObjects.ContainsKey(path))
        {
            return m_GameObjects[path] as T;
        }
        T obj = Resources.Load<T>(path);
        m_GameObjects.Add(path, obj);
        //m_AllGameObjects.Add(path, new List<Object> { obj });
        return obj;
    }

    public static List<T> LoadAll<T>(string path,string key)
    {
        if (m_AllGameObjects.ContainsKey(key))
        {
            return m_AllGameObjects[key] as List<T>;
        }
        List<Object> objs = Resources.LoadAll<Object>(path).ToList();
        m_AllGameObjects.Add(key, objs);
        return objs as List<T>;
    }

    public static Object GetGameObject(string path)
    {
        return m_GameObjects[path];
    }

    public static List<Object> GetGameObjectList(string key)
    {
        return m_AllGameObjects[key];
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

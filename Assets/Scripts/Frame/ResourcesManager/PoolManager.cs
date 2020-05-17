using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolType
{
    Window,
    Collider,
    Effect,
    EmitterManager,
    Emitter,
    Bullet,
}

public static class PoolManager
{
    static Dictionary<PoolType, Dictionary<string, PoolSpawn>> m_PoolDict = new Dictionary<PoolType, Dictionary<string, PoolSpawn>>();

    public static GameObject InstantiateGameObject(string path, PoolType pType)
    {
        GameObject go = ResourceManager.Load<GameObject>(path);
        if (!m_PoolDict.ContainsKey(pType))
        {
            m_PoolDict.Add(pType, new Dictionary<string, PoolSpawn>());
        }
        try
        {
            if (!m_PoolDict[pType].ContainsKey(go.name))
            {
                var pool = new PoolSpawn(go);
                m_PoolDict[pType].Add(go.name, pool);
            }
        }
        catch (System.Exception)
        {

            Debug.LogError(go.name);
            foreach (var item in m_PoolDict)
            {
                Debug.LogError(item);
            }
        }
        return m_PoolDict[pType][go.name].GetGameObject();
    }

    public static GameObject InstantiateGameObject(GameObject go, PoolType pType)
    {
        if (!m_PoolDict.ContainsKey(pType))
        {
            m_PoolDict.Add(pType, new Dictionary<string, PoolSpawn>());
        }
        if (!m_PoolDict[pType].ContainsKey(go.name))
        {
            var pool = new PoolSpawn(go);
            m_PoolDict[pType].Add(go.name, pool);
        }
        return m_PoolDict[pType][go.name].GetGameObject();
    }

    public static void DestroyGameObject(GameObject go,PoolType pType)
    {
        m_PoolDict[pType][go.name].DestroyGameObject(go);
    }
}

public class PoolSpawn
{
    Stack<GameObject> m_ObjStack;
    GameObject m_Prefab;
    public PoolSpawn(GameObject prefab)
    {
        m_ObjStack = new Stack<GameObject>();
        m_Prefab = prefab;
    }

    public GameObject GetGameObject()
    {
        GameObject go = null;
        if (m_ObjStack.Count>0)
        {
            go= m_ObjStack.Pop();
            go.SetActive(true);
            return go;
        }
        go = GameObject.Instantiate(m_Prefab);
        go.name = m_Prefab.name;
        return go;
    }

    public void DestroyGameObject(GameObject go)
    {
        go.SetActive(false);
        m_ObjStack.Push(go);
    }
}
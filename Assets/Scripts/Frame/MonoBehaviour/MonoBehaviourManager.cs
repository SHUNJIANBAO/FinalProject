using System.Collections.Generic;
using System.Linq;


public static class MonoBehaviourManager
{
    /// <summary>
    /// 管理的所有MonoBehaviour类
    /// </summary>
    static List<MonoEntity> m_CellList = new List<MonoEntity>();
    static bool m_IsInit = false;

    static void Init()
    {
        if (m_IsInit) return;
        m_IsInit = true;

        MonoBehaviourRuntime.Instance.OnUpdate += OnUpdate;
        MonoBehaviourRuntime.Instance.OnFixedUpdate += OnFixedUpdate;
        MonoBehaviourRuntime.Instance.OnLateUpdate += OnLateUpdate;
    }

    public static void Add(MonoEntity cell,params object[] objs)
    {
        Init();
        if (!m_CellList.Contains(cell))
        {
            cell.Init(objs);
            m_CellList.Add(cell);
            m_CellList.OrderByDescending(m => m.Index);
        }
    }
    public static void Remove(MonoEntity cell)
    {
        Init();
        if (m_CellList.Contains(cell))
            m_CellList.Remove(cell);
    }

    static void OnUpdate()
    {
        if (GameManager.IsPause) return;
        for (int i = 0; i < m_CellList.Count; i++)
        {
            m_CellList[i].CellUpdate();
        }
    }

    static void OnFixedUpdate()
    {
        if (GameManager.IsPause) return;
        for (int i = 0; i < m_CellList.Count; i++)
        {
            m_CellList[i].CellFixedUpdate();
        }
    }

    static void OnLateUpdate()
    {
        if (GameManager.IsPause) return;
        for (int i = 0; i < m_CellList.Count; i++)
        {
            m_CellList[i].CellLateUpdate();
        }
    }
}

using UnityEngine;

public class ShootManager : Singleton<ShootManager>
{
    /// <summary>
    /// 射击
    /// </summary>
    /// <param name="parent">射击父物体</param>
    /// <param name="pos">世界坐标位置</param>
    /// <param name="dir">射击方向</param>
    /// <param name="bullet">子弹</param>
    /// <param name="shootType">射击类型</param>
    /// <param name="count">每波数量</param>
    /// <param name="birthOffest">偏移值</param>
    /// <param name="birthIntervalTime">发射器生成间隔时间</param>
    /// <param name="birthIntervalDistance">发射器生成间隔距离或角度</param>
    /// <param name="wave">射击波数</param>
    /// <param name="intervalTime">每波间隔</param>
    /// <returns></returns>
    public EmitterManager Shoot(Transform parent, Vector3 pos, Vector3 dir, GameObject bullet,E_ShootType shootType,int count,float birthOffest, float birthIntervalTime, float birthIntervalDistance, int wave, float intervalTime)
    {
        var emitter = CreateEmitterManager(PoolType.EmitterManager, parent, pos, dir);
        emitter.Init(bullet, shootType, count, birthOffest, birthIntervalTime, birthIntervalDistance, wave, intervalTime);
        emitter.Enable();

        return emitter;
    }

    EmitterManager CreateEmitterManager(PoolType pType,Transform parent, Vector3 pos, Vector3 dir)
    {
        var go = ResourceManager.Load<GameObject>(PathManager.GetEmitterPath(pType.ToString()));
        var emitterGo = PoolManager.InstantiateGameObject(go, pType);
        var emitter = emitterGo.GetComponent<EmitterManager>();
        emitter.transform.SetParent(parent);
        emitter.transform.position = pos;
        emitter.transform.right = dir;
        return emitter;
    }
}

public enum E_ShootType
{
    Parallel,//平行
    Sector,//扇形
    Recursion,//递归
}

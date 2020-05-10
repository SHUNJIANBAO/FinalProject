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
    public EmitterManager Shoot(Transform parent, Vector3 pos, GameObject bullet, float bulletDamage, BarrageConfig barrageCfg, int targetLayer)
    {
        Vector3 dir = Vector3.zero;
        switch (barrageCfg.Direction)
        {
            case E_Direction.Up:
                dir = Vector3.up;
                break;
            case E_Direction.Down:
                dir = Vector3.down;
                break;
            case E_Direction.Left:
                dir = Vector3.left;
                break;
            case E_Direction.Right:
                dir = Vector3.right;
                break;
        }
        var emitter = CreateEmitterManager(PoolType.EmitterManager, parent, pos);
        emitter.Init(bullet, false, dir, bulletDamage, barrageCfg, targetLayer);
        emitter.Enable();

        return emitter;
    }

    public EmitterManager Shoot(Transform parent, GameObject owner, GameObject bullet, float bulletDamage, BarrageConfig barrageCfg, int targetLayer)
    {
        Vector3 dir = Vector3.zero;
        switch (barrageCfg.Direction)
        {
            case E_Direction.Up:
                dir = Vector3.up;
                break;
            case E_Direction.Down:
                dir = Vector3.down;
                break;
            case E_Direction.Left:
                if (owner.transform.localScale.x > 0)
                {
                    dir = Vector3.left;
                }
                else
                {
                    dir = Vector3.right;
                }
                break;
            case E_Direction.Right:
                if (owner.transform.localScale.x > 0)
                {
                    dir = Vector3.right;
                }
                else
                {
                    dir = Vector3.left;
                }
                break;
        }
        var emitter = CreateEmitterManager(PoolType.EmitterManager, parent, owner.transform.position);
        emitter.Init(bullet, owner.transform.localScale.x > 0, dir, bulletDamage, barrageCfg, targetLayer);
        emitter.Enable();
        return emitter;
    }

    EmitterManager CreateEmitterManager(PoolType pType, Transform parent, Vector3 pos)
    {
        var go = ResourceManager.Load<GameObject>(PathManager.GetEmitterPath(pType.ToString()));
        var emitterGo = PoolManager.InstantiateGameObject(go, pType);
        var emitter = emitterGo.GetComponent<EmitterManager>();
        emitter.transform.SetParent(parent, false);
        emitter.transform.position = pos;
        return emitter;
    }
}

public enum E_BarrageType
{
    Parallel = 0,//平行
    Sector,//扇形
    Recursion,//递归
}

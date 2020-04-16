using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitterManager : MonoBehaviour
{
    Coroutine _curCoroutine;
    List<Emitter> _emitterList = new List<Emitter>();

    GameObject _bullet;
    int _emitterCount;
    Vector2 _createEmitterOffest;
    float _bulletDamage;
    float _createEmitterIntervalTime;
    float _createEmitterIntervalDistance;
    E_BarrageType _shootType;

    int _shootWave;
    float _shootIntervalTime;

    public void Init(GameObject bullet,float bulletDamage, BarrageConfig barrageCfg)
    {
        _bullet = bullet;
        _bulletDamage = bulletDamage;
        _emitterCount = barrageCfg.Count;
        _shootType = barrageCfg.BarrageType;
        _createEmitterOffest = barrageCfg.Offest;
        _createEmitterIntervalTime = barrageCfg.BirthIntervalTime;
        _createEmitterIntervalDistance = barrageCfg.BirthIntervalDistance;

        _shootWave = barrageCfg.Wave;
        _shootIntervalTime = barrageCfg.ShootIntervalTime;
    }

    void InitEmitters()
    {
        switch (_shootType)
        {
            case E_BarrageType.Parallel:
                _curCoroutine = StartCoroutine(CreateParallelEmitter(_emitterCount, _createEmitterOffest, _createEmitterIntervalTime, _createEmitterIntervalDistance));
                break;
            case E_BarrageType.Sector:
                _curCoroutine = StartCoroutine(CreateSectorEmitter(_emitterCount, _createEmitterOffest, _createEmitterIntervalTime, _createEmitterIntervalDistance));
                break;
            case E_BarrageType.Recursion:
                _curCoroutine = StartCoroutine(CreateRecursionEmitter(_emitterCount, _createEmitterOffest, _createEmitterIntervalTime, _createEmitterIntervalDistance));
                break;
        }
    }

    Emitter CreateEmitter(Vector3 pos, Vector3 dir)
    {
        var go = ResourceManager.Load<GameObject>(PathManager.GetEmitterPath(PoolType.Emitter.ToString()));
        var emitterGo = PoolManager.InstantiateGameObject(go, PoolType.Emitter);
        emitterGo.transform.SetParent(transform, false);
        emitterGo.transform.localPosition = Vector3.zero;
        emitterGo.transform.right = dir;
        var emitter = emitterGo.GetComponentInChildren<Emitter>();
        emitter.transform.localPosition = pos;
        return emitter;
    }

    IEnumerator CreateParallelEmitter(int count, Vector2 offest, float intervalTime, float IntervalDistance)
    {
        int targetCount = count;

        float maxInterval = (count - 1) * IntervalDistance;
        Vector2 pos = new Vector2(0, maxInterval / 2) + offest;
        while (targetCount > 0)
        {
            var emitter = CreateEmitter(pos, transform.right);
            emitter.Init(_bullet, _bulletDamage, _shootWave, _shootIntervalTime);
            emitter.ShootStart();

            pos.y -= IntervalDistance;
            targetCount--;
            if (intervalTime != 0)
                yield return new WaitForSeconds(intervalTime);
        }
    }
    IEnumerator CreateSectorEmitter(int count, Vector2 offest, float intervalTime, float IntervalDistance)
    {
        int targetCount = count;
        float fullAngle = IntervalDistance * (count - 1);
        Vector3 rightDir = Quaternion.AngleAxis(fullAngle / 2, Vector3.forward) * transform.right;
        Quaternion rightQua = Quaternion.AngleAxis(-IntervalDistance, Vector3.forward);
        while (targetCount > 0)
        {
            var emitter = CreateEmitter(offest, rightDir);
            emitter.Init(_bullet, _bulletDamage, _shootWave, _shootIntervalTime);
            emitter.ShootStart();

            rightDir = rightQua * rightDir;
            targetCount--;
            if (intervalTime != 0)
                yield return new WaitForSeconds(intervalTime);
        }
    }
    IEnumerator CreateRecursionEmitter(int count, Vector2 offest, float intervalTime, float IntervalDistance)
    {
        int targetCount = count;
        Vector2 pos = offest;
        while (targetCount > 0)
        {
            var emitter = CreateEmitter(pos, transform.right);
            emitter.Init(_bullet, _bulletDamage, _shootWave, _shootIntervalTime);
            emitter.ShootStart();

            pos.x += IntervalDistance;
            targetCount--;
            if (intervalTime != 0)
                yield return new WaitForSeconds(intervalTime);
        }
    }

    /// <summary>
    /// 激活
    /// </summary>
    public void Enable()
    {
        InitEmitters();
    }

    /// <summary>
    /// 关闭
    /// </summary>
    public void Disable()
    {
        StopCoroutine(_curCoroutine);
        for (int i = 0; i < _emitterList.Count; i++)
        {
            _emitterList[i].ShootStop();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitterManager : MonoBehaviour
{
    Coroutine _curCoroutine;
    List<Emitter> _emitterList = new List<Emitter>();

    GameObject _bullet;
    int _emitterCount;
    float _createEmitterOffest;
    float _createEmitterIntervalTime;
    float _createEmitterIntervalDistance;
    E_ShootType _shootType;

    int _shootWave;
    float _shootIntervalTime;

    public void Init(GameObject bullet, E_ShootType shootType, int count, float birthOffest, float birthIntervalTime, float birthIntervalDistance, int wave, float intervalTime)
    {
        _bullet = bullet;
        _emitterCount = count;
        _shootType = shootType;
        _createEmitterOffest = birthOffest;
        _createEmitterIntervalTime = birthIntervalTime;
        _createEmitterIntervalDistance = birthIntervalDistance;
        _shootIntervalTime = intervalTime;

        _shootWave = wave;
        _shootIntervalTime = intervalTime;
    }

    void InitEmitters()
    {
        switch (_shootType)
        {
            case E_ShootType.Parallel:
                _curCoroutine = StartCoroutine(CreateParallelEmitter(_emitterCount, _createEmitterOffest, _createEmitterIntervalTime, _createEmitterIntervalDistance));
                break;
            case E_ShootType.Sector:
                _curCoroutine = StartCoroutine(CreateSectorEmitter(_emitterCount, _createEmitterOffest, _createEmitterIntervalTime, _createEmitterIntervalDistance));
                break;
            case E_ShootType.Recursion:
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

    IEnumerator CreateParallelEmitter(int count, float offest, float intervalTime, float IntervalDistance)
    {
        int targetCount = count;

        float maxInterval = (count - 1) * IntervalDistance;
        Vector3 pos = new Vector3(0, maxInterval / 2, 0);
        while (targetCount > 0)
        {
            var emitter = CreateEmitter(pos, transform.right);
            emitter.Init(_bullet, _shootWave, _shootIntervalTime);
            emitter.ShootStart();

            pos.y -= IntervalDistance;
            targetCount--;
            if (intervalTime != 0)
                yield return new WaitForSeconds(intervalTime);
        }
    }
    IEnumerator CreateSectorEmitter(int count, float offest, float intervalTime, float IntervalDistance)
    {
        int targetCount = count;
        Vector2 pos = new Vector2(offest, 0);
        float fullAngle = IntervalDistance * (count - 1);
        Vector3 rightDir = Quaternion.AngleAxis(fullAngle / 2, Vector3.forward) * transform.right;
        Quaternion rightQua = Quaternion.AngleAxis(-IntervalDistance, Vector3.forward);
        while (targetCount > 0)
        {
            var emitter = CreateEmitter(pos, rightDir);
            emitter.Init(_bullet, _shootWave, _shootIntervalTime);
            emitter.ShootStart();

            rightDir = rightQua * rightDir;
            targetCount--;
            if (intervalTime != 0)
                yield return new WaitForSeconds(intervalTime);
        }
    }
    IEnumerator CreateRecursionEmitter(int count, float offest, float intervalTime, float IntervalDistance)
    {
        int targetCount = count;
        Vector2 pos = new Vector2(offest, 0);
        while (targetCount > 0)
        {
            var emitter = CreateEmitter(pos, transform.right);
            emitter.Init(_bullet, _shootWave, _shootIntervalTime);
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

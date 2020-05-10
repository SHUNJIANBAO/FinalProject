using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitterManager : MonoBehaviour
{
    Coroutine _curCoroutine;
    List<Emitter> _emitterList = new List<Emitter>();

    public System.Action OnUpdate;

    GameObject _bullet;
    bool _isFaceRight;
    Vector3 _bulletDir;
    int _emitterCount;
    Vector2 _createEmitterOffest;
    float _bulletDamage;
    float _createEmitterIntervalTime;
    float _createEmitterIntervalDistance;
    E_BarrageType _shootType;
    int _targetLayer;

    int _shootWave;
    float _shootIntervalTime;

    public void Init(GameObject bullet, bool isFaceRight, Vector3 bulletDir, float bulletDamage, BarrageConfig barrageCfg, int targetLayer)
    {
        _bullet = bullet;
        _bulletDir = bulletDir;
        _isFaceRight = isFaceRight;
        _targetLayer = targetLayer;
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

    Emitter CreateEmitter(Vector3 pos)
    {
        var go = ResourceManager.Load<GameObject>(PathManager.GetEmitterPath(PoolType.Emitter.ToString()));
        var emitterGo = PoolManager.InstantiateGameObject(go, PoolType.Emitter);
        emitterGo.transform.SetParent(transform, false);
        emitterGo.transform.localPosition = Vector3.zero;
        var emitter = emitterGo.GetComponentInChildren<Emitter>();
        emitter.transform.localPosition = pos;
        return emitter;
    }

    IEnumerator CreateParallelEmitter(int count, Vector2 offest, float intervalTime, float IntervalDistance)
    {
        int targetCount = count;

        float maxInterval = (count - 1) * IntervalDistance;
        Vector2 pos = Vector2.zero;
        if (transform.parent == null)
        {
            if (_isFaceRight)
            {
                pos = new Vector2(0, maxInterval / 2) + offest;
            }
            else
            {
                pos = new Vector2(0, maxInterval / 2) + new Vector2(-offest.x, offest.y);
            }
        }
        else
        {
            pos = new Vector2(0, maxInterval / 2) + offest;
        }
        List<Emitter> emitterList = new List<Emitter>();
        while (targetCount > 0)
        {
            var emitter = CreateEmitter(pos);
            emitter.Init(_bullet, _bulletDir, _bulletDamage, _shootWave, _shootIntervalTime, _targetLayer);
            emitter.ShootStart();
            emitterList.Add(emitter);

            pos.y -= IntervalDistance;
            targetCount--;
            if (intervalTime != 0)
                yield return new WaitForSeconds(intervalTime);
        }
        bool shootEnd = false;
        do
        {
            var emitter = emitterList.Find(e => e.ShootEnd == false);
            if (emitter == null)
            {
                shootEnd = true;
            }
            yield return null;
        } while (!shootEnd);
        for (int i = 0; i < emitterList.Count; i++)
        {
            PoolManager.DestroyGameObject(emitterList[i].transform.parent.gameObject, PoolType.Emitter);
        }
        PoolManager.DestroyGameObject(gameObject, PoolType.EmitterManager);
    }
    IEnumerator CreateSectorEmitter(int count, Vector2 offest, float intervalTime, float IntervalDistance)
    {
        int targetCount = count;
        float fullAngle = IntervalDistance * (count - 1);
        Vector3 rightDir = Quaternion.AngleAxis(fullAngle / 2, Vector3.forward) * transform.right;
        Quaternion rightQua = Quaternion.AngleAxis(-IntervalDistance, Vector3.forward);
        List<Emitter> emitterList = new List<Emitter>();
        while (targetCount > 0)
        {
            var emitter = CreateEmitter(offest);
            emitter.Init(_bullet, _bulletDir, _bulletDamage, _shootWave, _shootIntervalTime, _targetLayer);
            emitter.ShootStart();
            emitterList.Add(emitter);

            rightDir = rightQua * rightDir;
            targetCount--;
            if (intervalTime != 0)
                yield return new WaitForSeconds(intervalTime);
        }
        bool shootEnd = false;
        do
        {
            var emitter = emitterList.Find(e => e.ShootEnd == false);
            if (emitter == null)
            {
                shootEnd = true;
            }
            yield return null;
        } while (!shootEnd);
        for (int i = 0; i < emitterList.Count; i++)
        {
            PoolManager.DestroyGameObject(emitterList[i].transform.parent.gameObject, PoolType.Emitter);
        }
        PoolManager.DestroyGameObject(gameObject, PoolType.EmitterManager);
    }
    IEnumerator CreateRecursionEmitter(int count, Vector2 offest, float intervalTime, float IntervalDistance)
    {
        int targetCount = count;
        Vector2 pos = offest;
        List<Emitter> emitterList = new List<Emitter>();
        while (targetCount > 0)
        {
            var emitter = CreateEmitter(pos);
            emitter.Init(_bullet, _bulletDir, _bulletDamage, _shootWave, _shootIntervalTime, _targetLayer);
            emitter.ShootStart();
            emitterList.Add(emitter);

            pos.x += IntervalDistance;
            targetCount--;
            if (intervalTime != 0)
                yield return new WaitForSeconds(intervalTime);
        }
        bool shootEnd = false;
        do
        {
            var emitter = emitterList.Find(e => e.ShootEnd == false);
            if (emitter == null)
            {
                shootEnd = true;
            }
            yield return null;
        } while (!shootEnd);
        for (int i = 0; i < emitterList.Count; i++)
        {
            PoolManager.DestroyGameObject(emitterList[i].transform.parent.gameObject, PoolType.Emitter);
        }
        PoolManager.DestroyGameObject(gameObject, PoolType.EmitterManager);
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

    private void Update()
    {
        OnUpdate?.Invoke();
    }
    private void OnDisable()
    {
        OnUpdate = null;
    }
}


using System.Collections;
using UnityEngine;

public class Emitter : MonoBehaviour
{
    public bool ShootEnd = false;

    GameObject _bullet;
    Vector3 _bulletDir;
    int _wave;
    float _intervalTime;
    float _bulletDamage;
    int _targetLayer;

    float _timeCount;
    public void Init(GameObject bullet, Vector3 bulletDir, float bulletDamage, int wave, float intervalTime, int targetLayer)
    {
        ShootEnd = false;
        _bullet = bullet;
        _bulletDir = bulletDir;
        _targetLayer = targetLayer;
        _bulletDamage = bulletDamage;
        _wave = wave;
        _intervalTime = intervalTime;
    }

    public void ShootStart()
    {
        StartCoroutine(ShootStartIE());
    }

    IEnumerator ShootStartIE()
    {
        while (_wave > 0)
        {
            var bullet = PoolManager.InstantiateGameObject(_bullet, PoolType.Bullet);
            MonoBehaviourManager.Add(bullet.GetComponent<BulletBase>(), _bulletDamage, _targetLayer);
            bullet.transform.position = transform.position;
            bullet.transform.right = _bulletDir;
            _wave--;
            if (_intervalTime != 0)
            {
                float timeCount = 0;
                while (timeCount < _intervalTime)
                {
                    timeCount += Time.deltaTime * (GameManager.IsTimeStop ? 0 : 1);
                    yield return null;
                }
            }
            //yield return new WaitForSeconds(_intervalTime);
        }
        ShootEnd = true;
    }

    public void ShootStop() { }
}

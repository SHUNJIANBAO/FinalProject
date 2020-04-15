
using System.Collections;
using UnityEngine;

public class Emitter : MonoBehaviour
{
    GameObject _bullet;
    int _wave;
    float _intervalTime;

    float _timeCount;
    public void Init(GameObject bullet, int wave, float intervalTime)
    {
        _bullet = bullet;
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
            MonoBehaviourManager.Add(bullet.GetComponent<BulletBase>());
            bullet.transform.position = transform.position;
            bullet.transform.right = transform.right;
            _wave--;
            yield return new WaitForSeconds(_intervalTime);
        }
    }

    public void ShootStop() { }
}

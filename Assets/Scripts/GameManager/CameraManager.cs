using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    static Camera _mainCamera;
    static CinemachineConfiner _cameraConfiner;
    static CinemachineVirtualCamera _cinemachine;
    static CinemachineFramingTransposer _body;
    static RippleEffect _cameraRippleEffect;

    private void Start()
    {
        _mainCamera = GetComponentInChildren<Camera>();
        _cameraConfiner = GetComponentInChildren<CinemachineConfiner>();
        _cinemachine = GetComponentInChildren<CinemachineVirtualCamera>();
        _cameraRippleEffect = GetComponentInChildren<RippleEffect>();
        foreach (var script in _cinemachine.GetComponentPipeline())
        {
            if (script is CinemachineFramingTransposer)
            {
                _body = script as CinemachineFramingTransposer;
                break;
            }
        }
        if (_body == null)
        {
            Debug.LogError("Body Con't Find!!!");
        }
    }

    public static void Ripple(Vector3 pos)
    {
        _cameraRippleEffect.Emit(_mainCamera.WorldToViewportPoint(pos));
    }

    public static bool IsMainCamera(Camera cam)
    {
        if (_mainCamera == null) return true;
        return _mainCamera == cam;
    }

    /// <summary>
    /// 设置摄像机活动范围
    /// </summary>
    /// <param name="collide"></param>
    public static void SetCameraConfiner(PolygonCollider2D collide)
    {
        if (_mainCamera == null) return;
        var trans = _cameraConfiner.gameObject;
        Destroy(_cameraConfiner);
        _cameraConfiner = trans.gameObject.AddComponent<CinemachineConfiner>();
        _cameraConfiner.m_BoundingShape2D = collide;
    }

    /// <summary>
    /// 设置跟随模式
    /// </summary>
    /// <param name="horizontal"></param>
    /// <param name="vertical"></param>
    public static void SetCameraFollowMode(bool horizontal, bool vertical)
    {
        if (_mainCamera == null) return;
        if (!horizontal)
        {
            _body.m_DeadZoneWidth = 1;
        }
        else
        {
            _body.m_DeadZoneWidth = 0f;
        }
        if (!vertical)
        {
            _body.m_DeadZoneHeight = 1;
        }
        else
        {
            _body.m_DeadZoneHeight = 0f;
        }
    }

    /// <summary>
    /// 设置跟随目标
    /// </summary>
    /// <param name="target"></param>
    public static void SetFollowTarget(Transform target)
    {
        if (_mainCamera == null) return;
        if (target!=null)
        {
            _cinemachine.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, _cinemachine.transform.position.z);
        }
        _cinemachine.m_Follow = target;
    }
}
